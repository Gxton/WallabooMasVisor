using Microsoft.AspNetCore.Authorization; // Importar para usar la autorización
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Wallaboo.Interfaces;
using Wallaboo.Models; // Importar el modelo de vista
using Wallaboo.Data; // Asegúrate de importar tu contexto de datos
using Microsoft.EntityFrameworkCore;
using Wallaboo.Entities; // Importar para manejar excepciones de EF

namespace Wallaboo.Controllers
{
    [ApiController] // Habilita la funcionalidad de API Controller
    [Route("api/[controller]")] // Establece la ruta base para las acciones del controlador
    public class MercadoPagoController : ControllerBase
    {
        private readonly IMercadoPagoService _mercadoPagoService;
        private readonly ApplicationDbContext _context; // Añade el contexto de la base de datos

        public MercadoPagoController(IMercadoPagoService mercadoPagoService, ApplicationDbContext context)
        {
            _mercadoPagoService = mercadoPagoService ?? throw new ArgumentNullException(nameof(mercadoPagoService));
            _context = context ?? throw new ArgumentNullException(nameof(context)); // Inicializa el contexto
        }

        [HttpGet("customerFind")] // Ruta para encontrar el customerId
        public async Task<IActionResult> GetCustomerId([FromQuery] string email)
        {
            try
            {
                var customerId = await _mercadoPagoService.GetCustomerIdByEmailAsync(email);

                if (customerId != null)
                {
                    return Ok(customerId);
                }
                else
                {
                    return NotFound($"No se encontró el customer_id para el email: {email}");
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al obtener el customer_id: {ex.Message}");
            }
        }

        [HttpPost("create")] // Ruta para crear un pago
        [Authorize] // Asegúrate de que esta acción requiere autenticación
        public async Task<IActionResult> CreatePayment([FromBody] PagoViewModel request) // Cambiar a [FromBody]
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Retorna los errores de validación
            }

            try
            {
                // Generar el cardToken
                var cardToken = await _mercadoPagoService.GenerateCardTokenAsync(request.CardNumber, request.ExpirationMonth, request.ExpirationYear, request.CardholderName, request.SecurityCode);

                // Obtener el customerId del email proporcionado
                var customerId = await _mercadoPagoService.GetCustomerIdByEmailAsync(request.Email);

                if (customerId == null)
                {
                    // Si no se encuentra el customerId, crear un nuevo cliente en MercadoPago
                    customerId = await _mercadoPagoService.CreateCustomerAsync(request.Email);
                }

                // Crear el pago utilizando los datos del modelo de vista y el cardToken generado
                var payment = await _mercadoPagoService.CreatePaymentAsync(request.Amount, request.Description, customerId, cardToken, request.SecurityCode, request.Email);

                // Verificar si el pago fue exitoso
                if (payment.Status == "approved") // Asegúrate de que esta condición se ajuste a la respuesta que recibes de Mercado Pago
                {
                    var anuncio = await _context.Anuncios.FindAsync(request.AnuncioId);
                    if (anuncio == null)
                    {
                        return NotFound();
                    }

                    // Modifico el estado de activo y pagado del anuncio
                    anuncio.Pagado = 1;
                    anuncio.Activo = 1;

                    _context.Update(anuncio);
                    await _context.SaveChangesAsync();

                    // Ingreso el pago en la tabla de pagos
                    var pago = new Pago // Asegúrate de que 'Pago' es el nombre correcto de tu modelo
                    {
                        total = request.Amount,
                        AnuncioID = anuncio.Id,
                        TenantId = anuncio.TenantId,
                        FechaPago = DateTime.Now
                    };

                    _context.Add(pago);
                    await _context.SaveChangesAsync();

                    return Ok(payment);
                }
                else
                {
                    return BadRequest("El pago no fue aprobado.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al procesar el pago: {ex.Message}");
            }
        }
    }
}



using ApiTask.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using NLog.Extensions.Logging;
using System.Text;

namespace ApiTask.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin, SubAdmin")]
    public class TransactionsController : ControllerBase
    {
        private readonly AppDbContext _dbContext;
        private readonly HttpClient _httpClient;
        private readonly ILogger<TransactionsController> _logger;

        public TransactionsController(AppDbContext dbContext, HttpClient httpClient, ILogger<TransactionsController> logger)
        {
            _dbContext = dbContext;
            _httpClient = httpClient;
            _logger = logger;
        }

        
        [HttpGet("list")]
        public async Task<List<Transaction>> GetTransactions()
        {
            var resp = HttpContext.Response.StatusCode;
            _logger.Log(LogLevel.Warning, $"Get All Transactions Successfully Request Status Code: {resp}");
            return await _dbContext.Transactions.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<Transaction> GetTransactionById(int id)
        {
            

            Transaction transaction = await _dbContext.Transactions.Where(t => t.TransactionId == id).FirstOrDefaultAsync();
            
            if (transaction == null)
                return new Transaction();

            var resp = HttpContext.Response.StatusCode;
            _logger.Log(LogLevel.Warning, $"Get Transaction By Id Successfully Request Status Code: {resp}");
            return transaction;
        }

        [HttpPost("create")]
        [Authorize(Roles = "Admin")]
        public async Task CreateTransaction([FromBody]Transaction request)
        {

            try {

                Transaction transaction = new Transaction()
                {
                    TransactionDate = DateTime.Now.Date.ToString("d"),
                    CurrencyType = request.CurrencyType,
                    CurrencyCode = request.CurrencyCode,
                    CurrencyName = request.CurrencyName,
                    CurrencyRate = await GetCurrencyRate(request.CurrencyType),
                    PaymentAmount = request.PaymentAmount,
                    RecievedAmount = request.RecievedAmount,
                    TransactionStatus = request.TransactionStatus,
                };

                _dbContext.Add(transaction);
                await _dbContext.SaveChangesAsync();

                var resp = HttpContext.Response;

                _logger.Log(LogLevel.Warning, $"Transaction Created Successfully Response Status Code: {resp.StatusCode}");

            }
            catch (Exception ex)
            {
                Console.WriteLine("", ex.Message);
            }
        }
        private async Task<decimal> GetCurrencyRate(string type)
        {

            var apiUrl = "https://api.apilayer.com/exchangerates_data/latest";
            _httpClient.DefaultRequestHeaders.Add("apikey", "vbjeljWWPNFs5FGuVoXeefSI6jdplMS1");
            var response = await _httpClient.GetAsync(apiUrl);
            var responseContent = await response.Content.ReadAsStringAsync();
            var json = JObject.Parse(responseContent);
            var rates = json["rates"];

            decimal value = 0;

            foreach (var rate in rates)
            {
                var curName = rate.First.Path;

                if (curName.Contains(type))
                {
                    value = rate.First.Value<decimal>();
                }
            }
            return value;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using XLantCore.Models;

namespace XLantDataStore.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IntelligentOfficeController : ControllerBase
    {
        private readonly ILogger<IntelligentOfficeController> _logger;
        private readonly Repository.IMLFSClientRepository _clientRepository;

        public IntelligentOfficeController(ILogger<IntelligentOfficeController> logger)
        {
            _logger = logger;
            _clientRepository = new Repository.MLFSClientRepository();
        }

        [HttpGet]
        [Route("GetClient")]
        public async Task<MLFSClient> GetClient(string clientId)
        {
            MLFSClient client = await _clientRepository.GetClient(clientId);
            return client;
        }

        [HttpGet]
        [Route("GetClients")]
        public async Task<List<MLFSClient>> GetClients(string[] clientIds)
        {
            List<MLFSClient> clients = new List<MLFSClient>();
            foreach(string id in clientIds)
            {
                MLFSClient client = await _clientRepository.GetClient(id);
                clients.Add(client);
            }
            return clients;
        }
    }
}

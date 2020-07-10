using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using XLantCore.Models;

namespace XLantDataStore.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class IntelligentOfficeController : ControllerBase
    {
        private readonly ILogger<IntelligentOfficeController> _logger;

        public IntelligentOfficeController(ILogger<IntelligentOfficeController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [Route("GetClient")]
        public MLFSClient GetClient(string clientId)
        {
            MLFSClient client = Repository.MLFSClientRepository.GetMLFSClient(clientId);
            return client;
        }

        [HttpGet]
        [Route("GetClients")]
        public List<MLFSClient> GetClients(string[] clientIds)
        {
            List<MLFSClient> clients = new List<MLFSClient>();
            foreach(string id in clientIds)
            {
                MLFSClient client = Repository.MLFSClientRepository.GetMLFSClient(id);
                clients.Add(client);
            }
            return clients;
        }
    }
}

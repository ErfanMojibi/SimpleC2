﻿using ApiModels.Requests;
using Microsoft.AspNetCore.Mvc;
using TeamServer.Services;
using TeamServer.Models.Listeners;
namespace TeamServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ListenersController : ControllerBase
    {
        private readonly IListenerService _listeners;
        private readonly IAgentService _agentService;
        public ListenersController(IListenerService listeners, IAgentService agentService)
        {
            _listeners = listeners;
            _agentService = agentService;
        }



        [HttpGet]
        public IActionResult GetAllListener()
        {

            return Ok(_listeners.GetAllListeners());
        }

        [HttpGet("{name}")]
        public IActionResult GetListener(string name)
        {
            var listener = _listeners.GetListener(name);
            if (listener == null) return NotFound();
            return Ok(listener);
        }


        [HttpPost]
        public IActionResult StartListener([FromBody] StartHttpListenerRequest request)
        {
            var listener = new HttpListener(request.Name, request.BindPort);
            listener.Init(_agentService);
            listener.Start();
            _listeners.AddListener(listener);

            var root = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}{HttpContext.Request.Path}";
            var path = $"{root}/{listener.Name}";

            return Created(path, listener);
        }

        [HttpDelete("{name}")]
        public IActionResult DeleteListener(string name)
        {
            var listener = _listeners.GetListener(name);
            if (listener == null)
            {
                return NotFound();
            }
            listener.Stop();

            _listeners.RemoveListener(listener);
            return NoContent();
        }

    }


    }


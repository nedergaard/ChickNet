using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ChickNetWeb.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GateController : ControllerBase
    {
        private readonly Gate.GateController _gateController;

        public GateController(ChickNetApp chickNetService)
        {
            _gateController = chickNetService.GateController;
        }

        // TODO : validate parameters
        [HttpPost("{id:int}")]
        public async Task<ActionResult<int>> DesiredState(int id, [FromBody] DesiredStateRequest desiredGateState)
        {
            // TODO : Fix Quick & Dirty input validation 
            if (id < 1 || id > 2 || desiredGateState.State == GateState.Undefined)
            {
                return new BadRequestResult();
            }

            bool isGateDesiredToBeOpen = desiredGateState.State == GateState.Open;
            if (isGateDesiredToBeOpen)
            {
                await _gateController.OpenGateAsync(id);
            }
            else
            {
                await _gateController.CloseGateAsync(id);
            }

            return id;
        }
    }

    public class DesiredStateRequest
    {
        public GateState State { get; set; }
    }

    public enum GateState
    {
        Undefined,
        Open,
        Closed,
    }
}

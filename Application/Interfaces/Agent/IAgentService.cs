using Application.DTOs.Agent;
using Application.ViewModels.Agent.Profile;
using Application.ViewModels.Agent.Properties;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces.Agent
{
    public interface IAgentService
    {
        Task<List<AgentPropertyDto>> GetPropertiesByAgentIdAsync(string agentId);
        Task<AgentPropertyDto> GetPropertyByIdAsync(string propertyId, string agentId);
        Task<string> CreatePropertyAsync(CreateAgentPropertyViewModel model, string agentId);
        Task UpdatePropertyAsync(string propertyId, string agentId, EditAgentPropertyViewModel model);
        Task DeletePropertyAsync(string propertyId, string agentId);
        Task<AgentProfileDto> GetAgentProfileAsync(string agentId);
        Task UpdateAgentProfileAsync(string agentId, UpdateAgentProfileDto model);
    }
}
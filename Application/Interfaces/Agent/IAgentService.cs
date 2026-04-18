using Application.DTOs.Agent;
using Application.ViewModels.Agent.Profile;
using Application.ViewModels.Agent.Properties;
using Application.ViewModels.Messages;
using Application.ViewModels.Offers;
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
        Task<List<AgentProfileDto>> GetAllAgentsAsync();
        Task<List<AgentProfileDto>> SearchAgentsByNameAsync(string name);
        Task<List<ChatViewModel>> GetChatsForAgentAsync(string agentId);
        Task<List<MessageViewModel>> GetMessagesByPropertyAsync(string propertyId, string clientId, string agentId);
        Task SendMessageAsync(SaveMessageViewModel vm);
        Task<List<OfferViewModel>> GetOffersByAgentAsync(string agentId);
        Task AcceptOfferAsync(string offerId);
        Task RejectOfferAsync(string offerId);
    }
}
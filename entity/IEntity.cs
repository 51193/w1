using MyGame.Component;
using System.Collections.Generic;

namespace MyGame.Entity
{
    public interface IEntity
    {
        public string GetRenderingGroupName();
        public void SetRenderingGroupName(string groupName);
        public string GetEntityName();
        public void InitiateStates(Dictionary<string, IState> states = null);
        public Dictionary<string, IState> GetStates();
        public void HandleStateTransition(string stateName, string input, params object[] args);
        public void EntityInitiateProcess();
    }
}

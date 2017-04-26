using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoowooTech.RajQueue.Managers
{
    public class ManagerCore
    {
        public static readonly ManagerCore Instance = new ManagerCore();
        private UserEvaluationManager _userEvaluationManager { get; set; }
        public UserEvaluationManager UserEvaluationManager { get { return _userEvaluationManager == null ? _userEvaluationManager = new UserEvaluationManager() : _userEvaluationManager; } }
    }
}
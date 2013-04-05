using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KeBot.Service.Bots;
using KeBot.Service.Bots.CheckinBot;
using Ninject.Modules;

namespace KeBot.Service
{
    public class BotsModule : NinjectModule
    {
        public override void Load()
        {
            //Bind<IBot>().To<DongBot>();
            //Bind<IBot>().To<FuckDongBot>();
            //Bind<IBot>().To<UrbanDictionaryBot>();
            //Bind<IBot>().To<HotpotBud>();
            Bind<IBot>().To<CheckinBot>();
            Bind(typeof(IRepository<>)).To(typeof(BotsRepository<>));
        }
    }
}

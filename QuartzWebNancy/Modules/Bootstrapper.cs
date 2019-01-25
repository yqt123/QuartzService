using Nancy;
using Nancy.Conventions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuartzWebNancy.Modules
{
    public class Bootstrapper : DefaultNancyBootstrapper
    {

        protected override void ConfigureConventions(NancyConventions nancyConventions)
        {
            base.ConfigureConventions(nancyConventions);
            nancyConventions.StaticContentsConventions.Clear();

            nancyConventions.StaticContentsConventions.Add(StaticContentConventionBuilder.AddDirectory("Content"));
        }
    }
}
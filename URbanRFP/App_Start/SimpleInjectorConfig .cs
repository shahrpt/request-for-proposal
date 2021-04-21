using System.Data;
using System.Data.Common;
using System.Web.Mvc;
using UrbanRFP.Facade.Facade;
using UrbanRFP.Facade.Interfaces;
using UrbanRFP.Infrastructure.Interfaces;
using UrbanRFP.Infrastructure.Repositories;
using System.Data.SqlClient;
using SimpleInjector;
using UrbanRFP.Helpers;
using SimpleInjector.Lifestyles;
using SimpleInjector.Integration.Web;
using SimpleInjector.Integration.Web.Mvc;

namespace UrbanRFP
{
    public static class SimpleInjectorConfig
    {
        public static Container container = new Container();

        public static void RegisterComponents()
        {
            string connectionString = Configuration.ConnectionString();
            
            container.Options.DefaultScopedLifestyle = new WebRequestLifestyle();

            container.Register<IDbConnection>(() => new SqlConnection(connectionString), Lifestyle.Scoped);


            #region Facade
            container.Register<IGeneralFacade, GeneralFacade>(Lifestyle.Scoped);
            container.Register<ISearchFacade, SearchFacade>(Lifestyle.Scoped);
            container.Register<IProductTypeFacade, ProductTypeFacade>(Lifestyle.Scoped);
            container.Register<IProductSubtypeFacade, ProductSubtypeFacade>(Lifestyle.Scoped);
            container.Register<IProductFacade, ProductFacade>(Lifestyle.Scoped);
            container.Register<IOrganizationFacade, OrganizationFacade>(Lifestyle.Scoped);
            container.Register<IAuthFacade, AuthFacade>(Lifestyle.Scoped);
            container.Register<IContactFacade, ContactFacade>(Lifestyle.Scoped);
            container.Register<IRfpRequestFacade, RfpRequestFacade>(Lifestyle.Scoped);
            container.Register<IRfpAttachmentFacade, RfpAttachmentFacade>(Lifestyle.Scoped);
            container.Register<IMiscFacade, MiscFacade>(Lifestyle.Scoped);
            container.Register<IScoreRuleFacade, ScoreRuleFacade>(Lifestyle.Scoped);

            #endregion

            #region DAL
            container.Register<IGeneralRepository, GeneralRepository>(Lifestyle.Scoped);
            container.Register<ISearchRepository, SearchRepository>(Lifestyle.Scoped);
            container.Register<IProductTypeRepository, ProductTypeRepository>(Lifestyle.Scoped);
            container.Register<IProductSubtypeRepository, ProductSubtypeRepository>(Lifestyle.Scoped);
            container.Register<IProductRepository, ProductRepository>(Lifestyle.Scoped);
            container.Register<IOrganizationRepository, OrganizationRepository>(Lifestyle.Scoped);
            container.Register<IContactRepository, ContactRepository>(Lifestyle.Scoped);
            container.Register<IRfpRequestRepository, RfpRequestRepository>(Lifestyle.Scoped);
            container.Register<IRfpAttachmentRepository, RfpAttachmentRepository>(Lifestyle.Scoped);
            container.Register<IMiscRepository, MiscRepository>(Lifestyle.Scoped);
            container.Register<IScoreRuleRepository, ScoreRuleRepository>(Lifestyle.Scoped);
            #endregion

            container.Verify();

            DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(container));
        }
    }
}
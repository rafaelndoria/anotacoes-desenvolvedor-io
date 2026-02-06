using DevIO.AppMvc.Mappings;
using DevIO.Business.Core.Notifications;
using DevIO.Business.Models.Fornecedores;
using DevIO.Business.Models.Fornecedores.Services;
using DevIO.Business.Models.Produtos;
using DevIO.Business.Models.Produtos.Services;
using DevIO.Infra.Data.Context;
using DevIO.Infra.Data.Repository;

using SimpleInjector;
using SimpleInjector.Integration.Web;
using SimpleInjector.Integration.Web.Mvc;

using System.Reflection;
using System.Web.Mvc;

namespace DevIO.AppMvc.App_Start
{
    public class DepedencyInjectionConfig
    {
        public static void RegisterDIContainer()
        {
            var container = new Container();
            container.Options.DefaultScopedLifestyle = new WebRequestLifestyle();

            InitializeContainers(container);

            container.RegisterMvcControllers(Assembly.GetExecutingAssembly());

            container.Verify();

            DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(container));
        }

        private static void InitializeContainers(Container container)
        {
            // Lifestyle.Singleton: Uma única instância é criada e compartilhada durante todo o tempo de vida da aplicação.
            // Lifestyle.Transient: Uma nova instância é criada cada vez que o serviço é solicitado.
            // Lifestyle.Scoped: Uma única instância é criada e compartilhada dentro do escopo definido (por exemplo, uma requisição web).

            container.Register<MeuDbContext>(Lifestyle.Scoped);
            container.Register<IProdutoRepository, ProdutoRepository>(Lifestyle.Scoped);
            container.Register<IProdutoService, ProdutoService>(Lifestyle.Scoped);
            container.Register<IFornecedorRepository, FornecedorRepository>(Lifestyle.Scoped); 
            container.Register<IEnderecoRepository, EnderecoRepository>(Lifestyle.Scoped);
            container.Register<IFornecedorService, FornecedorService>(Lifestyle.Scoped);
            container.Register<INotificador, Notificador>(Lifestyle.Scoped);

            container.Register<IMapperService, MapperService>(Lifestyle.Singleton);
        }
    }
}
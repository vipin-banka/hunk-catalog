using Plugin.Hunk.Catalog.Abstractions;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Customers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Plugin.Hunk.Catalog.ImportHandlers
{
    public abstract class CustomerImportHandler<TSourceEntity> : BaseEntityImportHandler<TSourceEntity, Customer>
        where TSourceEntity : IEntity
    {
        protected string AccountNumber { get; set; }

        protected string AccountStatus { get; set; }

        protected string Email { get; set; }

        protected string Password { get; set; }

        protected string FirstName { get; set; }

        protected string LastName { get; set; }

        protected string UserName { get; set; }

        protected IList<Tag> Tags { get; set; }

        protected CustomerImportHandler(string sourceEntity, CommerceCommander commerceCommander, CommercePipelineExecutionContext context)
        :base(sourceEntity, commerceCommander, context)
        {
        }

        public override async Task<CommerceEntity> Create()
        {
            Initialize();
            var command  = CommerceCommander.Command<CreateCustomerCommand>();
            var customer = new Customer();
            customer.AccountNumber = AccountNumber;
            customer.AccountStatus = AccountStatus;
            customer.Email = Email;
            customer.FirstName = FirstName;
            customer.LastName = LastName;
            customer.Password = Password;
            customer.UserName = UserName;
            customer.Tags = Tags;
            CommerceEntity = await command.Process(Context.CommerceContext, customer);
            return CommerceEntity;
        }

        public override IList<string> GetParentList()
        {
            return new List<string>();
        }
    }
}
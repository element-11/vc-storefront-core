﻿using System.Threading.Tasks;
using VirtoCommerce.Storefront.Model.Common.Events;
using VirtoCommerce.Storefront.Model.Customer;
using VirtoCommerce.Storefront.Model.Customer.Services;
using VirtoCommerce.Storefront.Model.Security.Events;

namespace VirtoCommerce.Storefront.Domain.Customer.Handlers
{
    public  class SecurityEventsHandler : IEventHandler<UserRegisteredEvent>, IEventHandler<UserDeletedEvent>
    {
        private readonly IMemberService _memberService;
        public SecurityEventsHandler(IMemberService memberService)
        {
            _memberService = memberService;
        }

        #region IEventHandler<UserRegisteredEvent> members
        public virtual async Task Handle(UserRegisteredEvent @event)
        {
            //Need to create new contact related to new user with same Id
            var registrationData = @event.UserRegistration;

            var contact = new Contact
            {
                Id = @event.User.Id,
                Name = registrationData.Name ?? registrationData.UserName,
                FullName = string.Join(" ", registrationData.FirstName, registrationData.LastName),
                FirstName = registrationData.FirstName,
                LastName = registrationData.LastName,
                OrganizationId = registrationData.OrganizationId
            };
            if (!string.IsNullOrEmpty(registrationData.Email))
            {
                contact.Emails.Add(registrationData.Email);
            }
            if (string.IsNullOrEmpty(contact.FullName) || string.IsNullOrWhiteSpace(contact.FullName))
            {
                contact.FullName = registrationData.Email;
            }
            if (registrationData.Address != null)
            {
                contact.Addresses = new[] { registrationData.Address };
            }
            //Try to register organization first
            if (string.IsNullOrEmpty(registrationData.OrganizationId) && !string.IsNullOrEmpty(registrationData.OrganizationName))
            {
                var organization = new Organization
                {
                    Name = registrationData.OrganizationName,
                    Addresses = contact.Addresses
                };               
                await _memberService.CreateOrganizationAsync(organization);
                contact.Organization = organization;
                contact.OrganizationId = organization.Id;
            }
            await _memberService.CreateContactAsync(contact);
        }

        public async Task Handle(UserDeletedEvent message)
        {
            if(message.User.ContactId != null)
            {
                await _memberService.DeleteContactAsync(message.User.ContactId);
            }
        }
        #endregion
    }
}

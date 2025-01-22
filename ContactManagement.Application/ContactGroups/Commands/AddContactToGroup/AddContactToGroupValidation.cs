using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactManagement.Application.ContactGroups.Commands.AddContactToGroup
{
    public class AddContactToGroupCommandValidator : AbstractValidator<AddContactToGroupCommand>
    {
        public AddContactToGroupCommandValidator()
        {
            RuleFor(x => x.GroupId)
                .NotEmpty();

            RuleFor(x => x.ContactId)
                .NotEmpty();
        }
    }
}

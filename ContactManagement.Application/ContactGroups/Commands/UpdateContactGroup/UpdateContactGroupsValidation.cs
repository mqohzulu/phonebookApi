using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactManagement.Application.ContactGroups.Commands.UpdateContactGroup
{
    public class UpdateContactGroupCommandValidator : AbstractValidator<UpdateContactGroupCommand>
    {
        public UpdateContactGroupCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty();

            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(100)
                .Matches("^[a-zA-Z0-9 -_]*$")
                    .WithMessage("Group name can only contain letters, numbers, spaces, hyphens, and underscores");

            RuleFor(x => x.Description)
                .MaximumLength(500);
        }
    }

}

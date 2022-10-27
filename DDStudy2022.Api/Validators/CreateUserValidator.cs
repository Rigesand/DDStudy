using DDStudy2022.Api.Interfaces;
using DDStudy2022.Api.Models;
using FluentValidation;

namespace DDStudy2022.Api.Validators;

public class CreateUserValidator : AbstractValidator<CreateUserModel>
{
    public CreateUserValidator(IUserService userService)
    {
        RuleFor(it => it.Email)
            .NotEmpty()
            .EmailAddress();
        RuleFor(it => it.Password)
            .MaximumLength(20)
            .NotEmpty();
        RuleFor(x => x)
            .Must(user => !userService.FindByMail(user.Email!).Result)
            .WithMessage("Email: Пользователь с таким email уже существует");
    }
}
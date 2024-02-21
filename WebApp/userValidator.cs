using FluentValidation;
using System;
using webApp.model;

namespace webApp
{
    public class userValidator : AbstractValidator<person>
    {
        public userValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty().WithMessage("firstname should be filled")
                                     .Length(0, 50).WithMessage("length should be between 0,50");
            RuleFor(x => x.LastName).NotEmpty().WithMessage("lastname should be filled")
                                  .Length(0, 50).WithMessage("length should be between 0 50");
            RuleFor(x => x.JobPosition).NotEmpty().WithMessage("jobposition should be filled")
                                  .Length(0, 50).WithMessage("length should be between 0 50");
            RuleFor(x => x.Salary).GreaterThan(0).LessThan(10000).WithMessage("salary should be between 0, 10000");
            RuleFor(x => x.WorkExperience).NotEmpty().WithMessage("workexperience should be filled");
            RuleFor(x => x.CreateDate < DateTime.Now);
            RuleFor(x => x.PersonAddress.City).NotEmpty().When(x => x.JobPosition is not null).WithMessage("city should be filled");
            RuleFor(x => x.PersonAddress.Country).NotEmpty().When(x => x.JobPosition is not null).WithMessage("country should be filled");
            RuleFor(x => x.PersonAddress.HomeNumber).NotEmpty().When(x => x.JobPosition is not null).WithMessage("homenumber should be filled");

        }
    }
}

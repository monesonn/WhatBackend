﻿using CharlieBackend.Business.Helpers;
using CharlieBackend.Core.DTO.Lesson;
using FluentValidation;

namespace CharlieBackend.Api.Validators.LessonDTOValidators
{
    public class FilterLessonsRequestDtoValidator : AbstractValidator<FilterLessonsRequestDto> 
    {
        public FilterLessonsRequestDtoValidator() //Is not necessary
        {
            RuleFor(x => x.StudentGroupId)
                .GreaterThan(0);
            RuleFor(x => x.FinishDate)
                   .Must((x, cancellation) => x.StartDate.HasValue && x.FinishDate.HasValue
                    && (x.FinishDate > x.StartDate || x.FinishDate.Equals(x.StartDate)))
                .When(x => x.FinishDate != null)
                .WithMessage(ValidationConstants.DatesNotValid);
        }
    }
}

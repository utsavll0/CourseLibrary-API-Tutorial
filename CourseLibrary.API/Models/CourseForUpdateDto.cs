using CourseLibrary.API.ValidationAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CourseLibrary.API.Models
{
    [CourseTitleMustBeDifferentFromDescription(ErrorMessage = "Title must be different than description")]
    public class CourseForUpdateDto : CourseForManipulationDto
    {
        [Required(ErrorMessage ="You should fill the description")]
        public override string Description { get => base.Description; set => base.Description = value; }
    }
}

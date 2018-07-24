using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace TestMakerFree.Data
{
    public class Quiz
    {
        #region Constructor
        public Quiz()
        {
            
        }
        #endregion
        #region Properties
        [Key]
        [Required]
        public int Id { get; set; }

        public string Description { get; set; }

        [Required]
        public string Title { get; set; }

        public string Text { get; set; }

        public string Notes { get; set; }

        [DefaultValue(0)]
        public int Type { get; set; }

        [DefaultValue(0)]
        public int Flags { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public int ViewCount { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        [Required]
        public DateTime LastModifiedDate { get; set; }
        #endregion

        #region Lazy-Load Properties
        /// <summary>
        /// The quiz author: it will be loaded
        /// on first use thanks to Lazy-Load feature of EF.
        /// </summary>

        [ForeignKey("UserId")]
        public virtual ApplicationUser User{ get; set; }

        /// <summary>
        /// A list containing all the related questions for a quiz
        /// </summary>
        public virtual List<Question> Questions{ get; set; }

        /// <summary>
        /// A list containing all the results related to the quiz
        /// </summary>
        public virtual List<Result> Results { get; set; }
        #endregion
    }
}
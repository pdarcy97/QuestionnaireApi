using System.Numerics;
using System.Runtime.CompilerServices;

namespace QuestionnaireApi.Models
{
    public class QuestionScores
    {
        public int QuestionId { get; set; }

        public int Min { get; set; }

        public int Max { get; set; }

        public int TotalScore { get; set; }

        public int Average
        {
            get
            {
                if (this.TotalScore > 0 && this.Responses > 0)
                {
                    return this.TotalScore / this.Responses;
                }

                return 0;

            }
        }

        public int Responses { get; set; }
    }
}

using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pizza.Utils;

namespace Pizza.Models
{
    public class SlicingChallengeResponse
    {
        static SlicingChallengeResponse()
        {
            Empty = new SlicingChallengeResponse();
        }

        public IList<Slice> ValidSlices { get; set; } = new List<Slice>();

        public int PointEarned => ValidSlices.Sum(slice => slice.Size);

        public static SlicingChallengeResponse Empty { get; }

        public string ToOutputString()
        {
            var sb = new StringBuilder();

            sb.AppendLine($"{ValidSlices.Count}");

            ValidSlices.ForEach(slice =>
            {
                sb.AppendLine($"{slice.TopLeftRow} {slice.TopLeftCol} {slice.BottomRightRow} {slice.BottomRightCol}");
            });

            return sb.ToString();
        }
    }
}
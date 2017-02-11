using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pizza.Models
{
    public class SlicingChallengeResponse
    {
        private SlicingChallengeResponse() {}

        public SlicingChallengeResponse(Slice validSlice)
        {
            ValidSlices.Add(validSlice);
        }

        public SlicingChallengeResponse(IEnumerable<Slice> slices )
        {
            ValidSlices.AddRange(slices);
        }
        static SlicingChallengeResponse()
        {
            Empty = new SlicingChallengeResponse();
        }

        public List<Slice> ValidSlices { get; } = new List<Slice>();

        public int PointEarned => ValidSlices.Sum(slice => slice.Size);

        public static SlicingChallengeResponse Empty { get; }

        public string ToOutputString()
        {
            var sb = new StringBuilder();

            sb.AppendLine($"{ValidSlices.Count}");

            ValidSlices.ForEach(slice =>
            {
                sb.AppendLine($"{slice.TopLeft.Y} {slice.TopLeft.X} {slice.BottomRight.Y} {slice.BottomRight.X}");
            });

            return sb.ToString();
        }
    }
}
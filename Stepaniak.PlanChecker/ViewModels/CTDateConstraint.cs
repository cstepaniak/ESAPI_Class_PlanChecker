using ESAPIX.Constraints;
using ESAPIX.Extensions;
using System;
using VMS.TPS.Common.Model.API;
using F = ESAPIX.Facade.API;  //UAB doesn't use facades any longer...they all break with ESAPI upgrades.

namespace Stepaniak.PlanChecker.ViewModels
{
    public class CTDateConstraint: IConstraint
    {
        public string Name => "CT Date < 60 days";

        public string FullName => "CT Date < 60 days";

        public ConstraintResult CanConstrain(PlanningItem pi) {
            //ESAPI version
            ////var hasImage = pi.StructureSet?.Image == null;  // short hand for if pi == null return null, else return StructureSet.Image
            //if (hasImage) { return new ConstraintResult(this, ResultType.PASSED, "");  }
            //else          { return new ConstraintResult(this, ResultType.NOT_APPLICABLE, "Missing Image");  }

            //ESAPIX version
            var pq = new PQAsserter(pi);
            return pq.HasImage().CumulativeResult;

        }

        public ConstraintResult Constrain(PlanningItem pi) {
            var image = pi.GetImage();
            return Constrain(new F.Image(image));
        }

        public ConstraintResult Constrain(F.Image image) {
            var diffDays = (DateTime.Now - image.CreationDateTime).Value.TotalDays;
            var msg = $"CT is {diffDays} days old";

            if (diffDays <= 60) {
                return new ConstraintResult(this, ResultType.PASSED, msg);
            } else {
                return new ConstraintResult(this, ResultType.ACTION_LEVEL_3, msg);
            }
        }
    }
}
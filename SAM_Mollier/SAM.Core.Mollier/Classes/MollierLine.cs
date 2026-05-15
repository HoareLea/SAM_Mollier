using System.Text.Json.Nodes;
namespace SAM.Core.Mollier
{
    public abstract class MollierLine : MollierCurve
    {
        internal MollierLine(MollierPoint mollierPoint)
            : base(mollierPoint)
        {

        }

        public MollierLine(MollierLine mollierLine)
            :base(mollierLine)
        {

        }

        public virtual bool FromJsonObject(JsonObject jObject)
        {
            bool result = base.FromJsonObject(jObject);
            if (!result)
            {
                return false;
            }

            return result;
        }

        public virtual JsonObject ToJsonObject()
        {
            JsonObject result = base.ToJsonObject();
            if(result == null)
            {
                return result;
            }

            return result;
        }
    }
}

namespace SAM.Analytical.Mollier
{
    public static partial class Create
    {
        public static Math.LinearEquation LinearEquation(this Core.Mollier.IMollierProcess mollierProcess)
        {
            if(mollierProcess == null)
            {
                return null;
            }
            return Geometry.Create.LinearEquation(mollierProcess.Start?.ToSAM_Point2D(), mollierProcess.End?.ToSAM_Point2D());
        }
    }
}

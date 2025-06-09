using System.ComponentModel;

namespace SAM.Core.Mollier
{
    /// <summary>
    /// Determines the applicable IAPWS-IF97 region for given pressure [MPa] and temperature [K].
    /// </summary>
    public enum IAPWSRegion
    {
        [Description("Undefined")] Undefined,
        
        /// <summary>
        /// Region 1 (compressed liquid water) 
        /// of the IAPWS-IF97 standard (valid for T ≤ 623.15 K and P ≤ 100 MPa).
        /// </summary>
        /// <remarks>
        /// Based on IAPWS Industrial Formulation 1997 for the Thermodynamic Properties of Water and Steam.
        /// Reference: https://www.iapws.org/relguide/IF97-Rev.pdf
        /// </remarks>
        [Description("Region1")] Region1,
        
        /// <summary>
        /// Region 2 (superheated steam) 
        /// of the IAPWS-IF97 standard (valid for 273.15 K ≤ T ≤ 1073.15 K and P ≤ 10 MPa).
        /// </summary>
        /// <remarks>
        /// Based on IAPWS Industrial Formulation 1997 for the Thermodynamic Properties of Water and Steam.
        /// Reference: https://www.iapws.org/relguide/IF97-Rev.pdf
        /// </remarks>
        [Description("Region2")] Region2,
        
        /// <summary>
        /// Region 3 (dense and near-critical water/steam) 
        /// of the IAPWS-IF97 standard.
        /// </summary>
        /// <remarks>
        /// Region 3 represents the high-density region of water/steam near the critical point.
        /// This implementation uses approximate placeholder methods for demonstration only.
        /// For validated calculations, use segmented formulations from the official IAPWS-IF97 Region 3 guide:
        /// https://www.iapws.org/relguide/IF97-Rev.pdf
        /// </remarks>
        [Description("Region3")] Region3,
        
        /// <summary>
        /// Region 4 (liquid-vapor boundary)
        /// based on the IAPWS-IF97 standard. Includes both empirical approximations and IAPWS-based calculations.
        /// </summary>
        /// <remarks>
        /// Region 4 defines the saturation curve (liquid-vapor equilibrium). 
        /// Saturation pressure and temperature are calculated using the Wagner & Pruß formulation.
        /// Reference: https://www.iapws.org/relguide/IF97-Rev.pdf
        /// </remarks>
        [Description("Region4")] Region4,

        /// <summary>
        /// Region 5 (ideal-gas-like superheated vapor)
        /// of the IAPWS-IF97 standard.
        /// </summary>
        /// <remarks>
        /// Region 5 covers high-temperature, low-pressure steam in the range of:
        /// - T = 1073.15 K to 2273.15 K
        /// - P ≤ 50 MPa
        /// Reference: IAPWS Industrial Formulation 1997 for the Thermodynamic Properties of Water and Steam
        /// https://www.iapws.org/relguide/IF97-Rev.pdf
        /// </remarks>
        [Description("Region5")] Region5
    }
}

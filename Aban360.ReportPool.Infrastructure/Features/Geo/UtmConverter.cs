namespace Aban360.ReportPool.Infrastructure.Features.Geo
{
    public static class UtmConverter
    {
        public static (double Easting, double Northing, int Zone, string Letter) LatLonToUtm(double lat, double lon)
        {
            // WGS84
            const double a = 6378137.0;
            const double f = 1.0 / 298.257223563;
            const double k0 = 0.9996;

            double e2 = f * (2 - f);
            double ePrime2 = e2 / (1 - e2);

            double latRad = lat * Math.PI / 180.0;
            double lonRad = lon * Math.PI / 180.0;

            int zone = (int)((lon + 180) / 6) + 1;
            string letter = GetLetter(lat);

            double lon0 = (zone - 1) * 6 - 180 + 3;
            double lon0Rad = lon0 * Math.PI / 180.0;

            double N = a / Math.Sqrt(1 - e2 * Math.Sin(latRad) * Math.Sin(latRad));
            double T = Math.Tan(latRad) * Math.Tan(latRad);
            double C = ePrime2 * Math.Cos(latRad) * Math.Cos(latRad);
            double A = Math.Cos(latRad) * (lonRad - lon0Rad);

            double M =
                a * ((1 - e2 / 4 - 3 * e2 * e2 / 64 - 5 * e2 * e2 * e2 / 256) * latRad
                - (3 * e2 / 8 + 3 * e2 * e2 / 32 + 45 * e2 * e2 * e2 / 1024) * Math.Sin(2 * latRad)
                + (15 * e2 * e2 / 256 + 45 * e2 * e2 * e2 / 1024) * Math.Sin(4 * latRad)
                - (35 * e2 * e2 * e2 / 3072) * Math.Sin(6 * latRad)
                );

            double easting =
                k0 * N * (A + (1 - T + C) * Math.Pow(A, 3) / 6
                + (5 - 18 * T + T * T + 72 * C - 58 * ePrime2) * Math.Pow(A, 5) / 120)
                + 500000.0;

            double northing =
                k0 * (M + N * Math.Tan(latRad)
                * (A * A / 2
                + (5 - T + 9 * C + 4 * C * C) * Math.Pow(A, 4) / 24
                + (61 - 58 * T + T * T + 600 * C - 330 * ePrime2) * Math.Pow(A, 6) / 720));

            if (lat < 0)
                northing += 10000000.0;

            return (easting, northing, zone, letter);
        }

        private static string GetLetter(double lat)
        {
            if (lat >= 84) return "X";
            if (lat >= 72) return "W";
            if (lat >= 64) return "V";
            if (lat >= 56) return "U";
            if (lat >= 48) return "T";
            if (lat >= 40) return "S";
            if (lat >= 32) return "R";
            if (lat >= 24) return "Q";
            if (lat >= 16) return "P";
            if (lat >= 8) return "N";
            if (lat >= 0) return "M";
            if (lat >= -8) return "L";
            if (lat >= -16) return "K";
            if (lat >= -24) return "J";
            if (lat >= -32) return "H";
            if (lat >= -40) return "G";
            if (lat >= -48) return "F";
            if (lat >= -56) return "E";
            if (lat >= -64) return "D";
            if (lat >= -72) return "C";
            return "Z";
        }
    }

}

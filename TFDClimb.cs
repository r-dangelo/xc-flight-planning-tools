using System;
using System.Collections.Generic;

public class TFDClimb
{
    int densityAlt1 = 0;
    int densityAlt2 = 0;

    double climbTime1 = 0;
    double climbTime2 = 0;
    double climbFuel1 = 0;
    double climbFuel2 = 0;
    double climbDist1 = 0;
    double climbDist2 = 0;

    Dictionary<int, double[]> climbValues = new Dictionary<int, double[]>();

    public TFDClimb(){
        buildDictionary();
    }

    public TFDClimb(int _oat1, int _pressAlt1, int _oat2, int _pressAlt2){
        buildDictionary();
        densityAlt1 = calcDensityAlt(_oat1, _pressAlt1);
        densityAlt2 = calcDensityAlt(_oat2, _pressAlt2);
    }

    // this is so horrible i wish i had the formula but thats also awful
    // im so sorry for this
    void buildDictionary(){
        // For Values: [0] is time, [1], is fuel, [2] is distance
        climbValues.Add(0, new double[] { 0, 0, 0 });
        climbValues.Add(1, new double[] { 1, 1, 1 });
        climbValues.Add(2, new double[] { 1.5, 2, 4 });
        climbValues.Add(3, new double[] { 2, 2.5, 5 });
        climbValues.Add(4, new double[] { 4.5, 3, 8 });
        climbValues.Add(5, new double[] { 8, 3.5, 11 });
        climbValues.Add(6, new double[] { 10, 4, 14 });
        climbValues.Add(7, new double[] { 11, 4.5, 16.5 });
        climbValues.Add(8, new double[] { 14, 5, 21 });
        climbValues.Add(9, new double[] { 18, 6, 28 });
        climbValues.Add(10, new double[] { 22, 7, 32 });
        climbValues.Add(11, new double[] { 26, 8, 39 });
        climbValues.Add(12, new double[] { 32, 9, 48 });
    }

    int calcDensityAlt(int OAT, int pressureAlt) {
        double _lapseRateHelper = Math.Round((double)pressureAlt / 1000.0);
        int _isaTemp = 15 - (int)(2 * _lapseRateHelper);

        return (int)(pressureAlt + (118.8 * (OAT - _isaTemp)));
    }

    void calcTimePoints(){
        int _lowerAltBound1 = 0;
        int _upperAltBound1 = 0;
        double _lowerValueBound1 = 0;
        double _upperValueBound1 = 0;
        int _dAltInfluence1 = 0;
        double _percentage1 = 0.0;

        int _lowerAltBound2 = 0;
        int _upperAltBound2 = 0;
        double _lowerValueBound2 = 0;
        double _upperValueBound2 = 0;
        int _dAltInfluence2 = 0;
        double _percentage2 = 0.0;

        // if density alt is a point, use the point
        if (climbValues.TryGetValue(densityAlt1, out double[] value)){
            climbTime1 = value[0];
            return;
        }

        if (climbValues.TryGetValue(densityAlt2, out double[] _value)) {
            climbTime2 = _value[0];
            return;
        }

        // select nearest 1000s and set values
        _lowerAltBound1 = densityAlt1/1000;
        _upperAltBound1 = _lowerAltBound1 + 1;
        _lowerValueBound1 = climbValues[_lowerAltBound1][0];
        _upperValueBound1 = climbValues[_upperAltBound1][0];

        _lowerAltBound2 = densityAlt2 / 1000;
        _upperAltBound2 = _lowerAltBound2 + 1;
        _lowerValueBound2 = climbValues[_lowerAltBound2][0];
        _upperValueBound2 = climbValues[_upperAltBound2][0];


        //find percentage dalt of altbounds range (always 1000)
        _dAltInfluence1 = densityAlt1 % 1000;
        _percentage1 = _dAltInfluence1 / 1000.0;

        _dAltInfluence2 = densityAlt2 % 1000;
        _percentage2 = _dAltInfluence2 / 1000.0;

        // take percentage of range
        climbTime1 = _lowerValueBound1 + (_percentage1 *
                    (_upperValueBound1 - _lowerValueBound1));

        climbTime2 = _lowerValueBound2 + (_percentage2 *
                    (_upperValueBound2 - _lowerValueBound2));

        TimeSpan _climbTime = TimeSpan.FromMinutes(climbTime2 - climbTime1);

        Console.WriteLine("Density alt 1: " + densityAlt1);
        Console.WriteLine("Density alt 2: " + densityAlt2);
        Console.WriteLine("");

        Console.WriteLine("TIME TO CLIMB:");
        Console.WriteLine("Climb Time Value 1: " + climbTime1);
        Console.WriteLine("Climb Time Value 2: " + climbTime2);
        Console.WriteLine("Climb Time: " + _climbTime.ToString("m\\:ss"));
        Console.WriteLine("");
    }

    void calcFuel()
    {
        // compare with fuel points to see which is more accurate
        // use linear regression to find, it's not perfect, but it overestimates
        climbFuel1 = 0.0162338 * ((densityAlt1/1000 + 15.0692) * (densityAlt1/1000 + 15.0692)) - 3.16441;
        climbFuel2 = 0.0162338 * ((densityAlt2/1000 + 15.0692) * (densityAlt2/1000 + 15.0692)) - 3.16441;
            
        Console.WriteLine("FUEL TO CLIMB:");
        Console.WriteLine("Climb Fuel 1: " + climbFuel1);
        Console.WriteLine("Climb Fuel 2: " + climbFuel2);
        Console.WriteLine("Climb Fuel: " + (climbFuel2 - climbFuel1));
    }

    void calcFuelPoints()
    {
        int _lowerAltBound1 = 0;
        int _upperAltBound1 = 0;
        double _lowerValueBound1 = 0;
        double _upperValueBound1 = 0;
        int _dAltInfluence1 = 0;
        double _percentage1 = 0.0;

        int _lowerAltBound2 = 0;
        int _upperAltBound2 = 0;
        double _lowerValueBound2 = 0;
        double _upperValueBound2 = 0;
        int _dAltInfluence2 = 0;
        double _percentage2 = 0.0;

        // if density alt is a point, use the point
        if (climbValues.TryGetValue(densityAlt1, out double[] value)) {
            climbFuel1 = value[1];
            return;
        }

        if (climbValues.TryGetValue(densityAlt2, out double[] _value)) {
            climbFuel2 = _value[1];
            return;
        }

        // select nearest 1000s and set values
        _lowerAltBound1 = densityAlt1 / 1000;
        _upperAltBound1 = _lowerAltBound1 + 1;
        _lowerValueBound1 = climbValues[_lowerAltBound1][1];
        _upperValueBound1 = climbValues[_upperAltBound1][1];

        _lowerAltBound2 = densityAlt2 / 1000;
        _upperAltBound2 = _lowerAltBound2 + 1;
        _lowerValueBound2 = climbValues[_lowerAltBound2][1];
        _upperValueBound2 = climbValues[_upperAltBound2][1];


        //find percentage dalt of altbounds range (always 1000)
        _dAltInfluence1 = densityAlt1 % 1000;
        _percentage1 = _dAltInfluence1 / 1000.0;

        _dAltInfluence2 = densityAlt2 % 1000;
        _percentage2 = _dAltInfluence2 / 1000.0;

        // take percentage of range
        climbFuel1 = _lowerValueBound1 + (_percentage1 *
                    (_upperValueBound1 - _lowerValueBound1));

        climbFuel2 = _lowerValueBound2 + (_percentage2 *
                    (_upperValueBound2 - _lowerValueBound2));

        Console.WriteLine("");
        Console.WriteLine("FUEL TO CLIMB Points:");
        Console.WriteLine("Climb Fuel 1: " + climbFuel1);
        Console.WriteLine("Climb Fuel 2: " + climbFuel2);
        Console.WriteLine("Climb Fuel: " + (climbFuel2 - climbFuel1));
    }

    void calcDistPoints()
    {
        int _lowerAltBound1 = 0;
        int _upperAltBound1 = 0;
        double _lowerValueBound1 = 0;
        double _upperValueBound1 = 0;
        int _dAltInfluence1 = 0;
        double _percentage1 = 0.0;

        int _lowerAltBound2 = 0;
        int _upperAltBound2 = 0;
        double _lowerValueBound2 = 0;
        double _upperValueBound2 = 0;
        int _dAltInfluence2 = 0;
        double _percentage2 = 0.0;

        // if density alt is a point, use the point
        if (climbValues.TryGetValue(densityAlt1, out double[] value)) {
            climbDist1 = value[2];
            return;
        }

        if (climbValues.TryGetValue(densityAlt2, out double[] _value)) {
            climbDist2 = _value[2];
            return;
        }

        // select nearest 1000s and set values
        _lowerAltBound1 = densityAlt1 / 1000;
        _upperAltBound1 = _lowerAltBound1 + 1;
        _lowerValueBound1 = climbValues[_lowerAltBound1][2];
        _upperValueBound1 = climbValues[_upperAltBound1][2];

        _lowerAltBound2 = densityAlt2 / 1000;
        _upperAltBound2 = _lowerAltBound2 + 1;
        _lowerValueBound2 = climbValues[_lowerAltBound2][2];
        _upperValueBound2 = climbValues[_upperAltBound2][2];


        //find percentage dalt of altbounds range (always 1000)
        _dAltInfluence1 = densityAlt1 % 1000;
        _percentage1 = _dAltInfluence1 / 1000.0;

        _dAltInfluence2 = densityAlt2 % 1000;
        _percentage2 = _dAltInfluence2 / 1000.0;

        // take percentage of range
        climbDist1 = _lowerValueBound1 + (_percentage1 *
                    (_upperValueBound1 - _lowerValueBound1));

        climbDist2 = _lowerValueBound2 + (_percentage2 *
                    (_upperValueBound2 - _lowerValueBound2));

        Console.WriteLine("");
        Console.WriteLine("Dist TO CLIMB Points:");
        Console.WriteLine("Climb Dist 1: " + climbDist1);
        Console.WriteLine("Climb Dist 2: " + climbDist2);
        Console.WriteLine("Climb Dist: " + (climbDist2 - climbDist1));
    }

    public static void Main(string[] args){
        TFDClimb test = new TFDClimb(23, 2000, 15, 6000);
        test.calcTimePoints();
        test.calcFuelPoints();
        test.calcDistPoints();
    }
}
using System;
using System.Collections.Generic;

public class CruisePerformance65
{
    int pressureAlt = 0;
    int OAT = 0;
    int RPM = 0;
    int TAS = 0;
    int densityAlt = 0;


    public CruisePerformance65(int _pressureAlt, int _OAT){
        pressureAlt = _pressureAlt;
        OAT = _OAT;
        densityAlt = calcDensityAlt(_OAT, _pressureAlt);
    }

    int calcDensityAlt(int OAT, int pressureAlt)
    {
        double _lapseRateHelper = Math.Round((double)pressureAlt / 1000.0);
        int _isaTemp = 15 - (int)(2 * _lapseRateHelper);

        return (int)(pressureAlt + (118.8 * (OAT - _isaTemp)));
    }

    void setRPMs()
    {
        int _tempRPM = ((-0.000277625 * densityAlt - 18.8995) * (-0.000277625 * densityAlt - 18.8995)) + 2046.42;

        RPM = 5 * (int)Math.Round(_tempRPM / 5.0);

        Console.WriteLine("RPMS: ");
        Console.WriteLine(RPM);

    }

    void setTAS()
    {
        // dont forget to subtract 3 for wheel pants
    }

    public static void Main(string[] args)
    {
        CruisePerformance65 test = new CruisePerformance65(6000, 13);
        test.setRPMs();
    }
}
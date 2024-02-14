using System;

public class CruisePerformance66
{
    int pressureAlt = 0;
    int OAT = 0;
    int RPM = 0;
    int TAS = 0;
    int densityAlt = 0;


    public CruisePerformance66(int _pressureAlt, int _OAT)
    {
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
        int _tempRPM = (int)(((-0.000277625 * densityAlt - 18.8995) * (-0.000277625 * densityAlt - 18.8995)) + 2046.42);

        RPM = 5 * (int)Math.Round(_tempRPM / 5.0);

        Console.WriteLine("RPMS: " + RPM);

    }

    void setTAS()
    {
        // subtract 3 for no wheel pants
        TAS = (int)(Math.Round(0.000501726 * densityAlt + 113.899)) - 3;
        Console.WriteLine("TAS: " + TAS);
    }

    public void findInfo()
    {
        Console.WriteLine("CRUISE PERFORMANCE:");
        setRPMs();
        setTAS();
        Console.WriteLine();
    }

}
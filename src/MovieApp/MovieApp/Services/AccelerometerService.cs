using System;
using Xamarin.Essentials;

namespace MovieApp
{
    // Based on: https://stackoverflow.com/a/42950329

    public class AccelerometerService : IAccelerometerService
    {
        private double sensorX;
        private double sensorY;
        private double firstSensorX = double.MinValue;
        private double firstSensorY = double.MinValue;
        private double previousSensorX;
        private double previousSensorY;
        private double movementMultiplier = 3;
        private double minMovedPixelsToUpdate = 2d;
        private float minSensibility = 0;

        private double translationX = 0;
        private double translationY = 0;

        public delegate void TranslationEventHandler(double x, double y);
        public event TranslationEventHandler TranslationChanged;


        public AccelerometerService()
        {
            Accelerometer.ReadingChanged += AccelerometerReadingChanged;
            Toggle();
        }


        private void AccelerometerReadingChanged(object sender, AccelerometerChangedEventArgs e)
        {
            sensorX = e.Reading.Acceleration.X * 9.81f;
            sensorY = -e.Reading.Acceleration.Y * 9.81f;

            ManageSensorValues();
        }

        private void SetNewPosition()
        {
            double destinyX = ((firstSensorX - sensorX) * movementMultiplier);
            double destinyY = ((firstSensorY - sensorY) * movementMultiplier);

            CalculateTranslationX(destinyX);
            CalculateTranslationY(destinyY);

            TranslationChanged?.Invoke(translationX, translationY);
        }

        private void CalculateTranslationX(double destinyX)
        {
            if (translationX + minMovedPixelsToUpdate < destinyX)
                translationX += Math.Abs(translationX - destinyX);
            else if (translationX - minMovedPixelsToUpdate > destinyX)
                translationX -= Math.Abs(translationX - destinyX);
        }

        private void CalculateTranslationY(double destinyY)
        {
            if (translationY + minMovedPixelsToUpdate < destinyY)
                translationY += minMovedPixelsToUpdate;
            else if (translationY - minMovedPixelsToUpdate > destinyY)
                translationY -= minMovedPixelsToUpdate;
        }

        private void ManageSensorValues()
        {
            if (firstSensorX.Equals(double.MinValue))
                SetFirstSensorValues();

            if (previousSensorX.Equals(double.MinValue) || IsSensorValuesMovedEnough())
            {
                SetNewPosition();
                SetPreviousSensorValues();
            }
        }

        private void SetFirstSensorValues()
        {
            firstSensorX = sensorX;
            firstSensorY = sensorY;
        }

        private void SetPreviousSensorValues()
        {
            previousSensorX = sensorX;
            previousSensorY = sensorY;
        }

        private bool IsSensorValuesMovedEnough()
        {
            return sensorX > previousSensorX + minSensibility || sensorX < previousSensorX - minSensibility
                || sensorY > previousSensorY + minSensibility || sensorY < previousSensorX - minSensibility;
        }

        private void Toggle()
        {
            try
            {
                if (!Accelerometer.IsMonitoring)
                    Accelerometer.Start(SensorSpeed.UI);
            }
            catch
            { }
        }
    }
}

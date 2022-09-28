using Plugin.Maui.Audio;

namespace MauiDiceRoller;

public partial class MainPage : ContentPage
{

	int randomValue;
	List<String> imageslist = new List<string>();
    String buttonstring;
    int i;
    private readonly IAudioManager audioManager;

    public MainPage(IAudioManager audioManager)
	{
		InitializeComponent();
		imageContent.Source = "dice1yellow.png"; 
		imageslist.Add("dice1yellow.png");
        imageslist.Add("dice2yellow.png");
        imageslist.Add("dice3yellow.png");
        imageslist.Add("dice4yellow.png");
        imageslist.Add("dice5yellow.png");
        imageslist.Add("dice6yellow.png");
        ToggleShake();
        this.audioManager = audioManager;
    }

    private async void RollDice()
    {
        TimeSpan vibrationLength = TimeSpan.FromSeconds(1);
        Vibration.Default.Vibrate(vibrationLength);

        var player = audioManager.CreatePlayer(await FileSystem.OpenAppPackageFileAsync("audio.mp3"));
        player.Play(); 

        randomValue = new Random().Next(6);

        buttonstring = $"You rolled: {randomValue + 1}";
        CounterBtn.Text = buttonstring;
        imageContent.Source = imageslist[randomValue];

        SemanticScreenReader.Announce(CounterBtn.Text);
        await TextToSpeech.Default.SpeakAsync(buttonstring);

        i = 0;
        for (i = 0; i <= randomValue; i++)
        {
            await Flashlight.Default.TurnOnAsync();
            await Task.Delay(200); 
            await Flashlight.Default.TurnOffAsync();
        }
    }

	private void OnCounterClicked(object sender, EventArgs e)
	{
        RollDice(); 
	}

    private void ToggleShake()
    {
        if (Accelerometer.Default.IsSupported)
        {
            if (!Accelerometer.Default.IsMonitoring)
            {
                // Turn on compass
                Accelerometer.Default.ShakeDetected += Accelerometer_ShakeDetected;
                Accelerometer.Default.Start(SensorSpeed.Game);
            }
        }
    }

    private void Accelerometer_ShakeDetected(object sender, EventArgs e)
    {
        //AudioManager.Current.CreatePlayer()
        //var audioPlayer = AudioManager.Current.CreatePlayer(await FileSystem.OpenAppPackageFileAsync("audio.mp3"));

        //audioPlayer.Play();

        //Task.Wait(1);  
        RollDice(); 
    }
}


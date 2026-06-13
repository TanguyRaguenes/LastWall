using Godot;
using System.Collections.Generic;

namespace GodotGame.Autoload.AudioManager;

public partial class AudioManager : Node
{

    private AudioStreamPlayer _sfxPlayer = null!;

    private readonly Dictionary<string, AudioStream> _sounds = new();

    private AudioStreamPlayer _musicPlayer = null!;


	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        GD.Print("AudioManager prêt");

        _sfxPlayer = GetNode<AudioStreamPlayer>("SfxPlayer");
        _musicPlayer = GetNode<AudioStreamPlayer>("MusicPlayer");

        _sounds["arrow"] = GD.Load<AudioStream>("res://Assets/Sounds/arrow-swish.mp3");
        
        // _musicPlayer.Stream = GD.Load<AudioStream>("res://Assets/Musics/OST 1 - Poison Ivy Manor (Loopable).mp3");

        _musicPlayer.Play();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void PlaySfx(string soundName)
    {
        if (!_sounds.TryGetValue(soundName, out AudioStream? sound))
        {
            GD.PrintErr($"Son introuvable : {soundName}");
            return;
        }

        _sfxPlayer.Stream = sound;
        _sfxPlayer.Play();
    }

}

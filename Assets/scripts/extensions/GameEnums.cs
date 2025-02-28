public enum WaveColor
{
	White,
	Black,
	Blue,
	Cyan,
	Grey,
	Green,
	Magenta,
	Red,
	Yellow
}

public enum DebugLogLevel
{
	VeryDetailed = 1, 
	MediumDetails = 2,
	OnlyImportant = 3,
	Off = 10,
}

public enum AlvildaState
{
	Idle,
	Occupied,
	Curious,
	Wandering,
	MovingTowardsSound,
	Dead
}

public enum WaveType
{
	Normal,
	Horn,
	Bell,
	Destructable,
	Lever,

    //The wave type for the sound object found on Train carts. 
    Traincart 

}


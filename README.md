# TheLongRoadHomeCharacterController

This script is a player controller script for a character in a Unity game. It handles character movement, rotation, animation, and input handling.

### Public Variables
- `walkSpeed`: The walking speed of the character.
- `runSpeed`: The running speed of the character.
- `rotationSpeed`: The rotation speed of the character.
- `acceleration`: The acceleration factor for character movement.
- `deceleration`: The deceleration factor for character movement.
- `aimingWalkSpeed`: The walking speed of the character while aiming.

### Private Variables
- `playerControls`: An instance of the `PlayerControls` class for handling input.
- `characterController`: A reference to the `CharacterController` component attached to the character.
- `animator`: A reference to the `Animator` component attached to the character.
- `currentMovementInput`: The current input for character movement.
- `currentMovement`: The current movement direction of the character.
- `currentRunMovement`: The current movement direction of the character while running.
- `currentMovementRaw`: The raw movement vector for the character.
- `currentRunMovementRaw`: The raw movement vector for the character while running.
- `movement`: The final movement vector for the character.
- `aimedMovement`: The input vector for aiming movement.
- `currentAimedMovement`: The current input vector for aiming movement.
- `isMovementPressed`: A flag indicating if movement input is being pressed.
- `isRunPressed`: A flag indicating if the run input is being pressed.
- `isAiming`: A flag indicating if the player is aiming.
- `cameraMainTransform`: A reference to the main camera's transform.

### Awake()
- Initializes the script by setting up event handlers for movement and run inputs.
- Retrieves references to the required components and sets the cursor lock state.

### Start()
- Initializes the `aimedMovement` vector to zero.

### Update()
- Updates the character's movement, rotation, and animation every frame.
- Calculates the movement vectors based on the camera orientation.
- Applies movement using `CharacterController.Move()`.
- Handles rotation based on movement input and aiming status.
- Updates animation based on movement and aiming inputs.

### GetAnimatorValues()
- Retrieves the current values from the animator, including the aiming status.

### OnRunPressed(InputAction.CallbackContext ctx)
- Event handler for the run input.
- Updates the `isRunPressed` flag based on the input state.

### HandleRotation()
- Handles the rotation of the character based on movement input.
- Calculates the target rotation based on the current movement direction.
- Smoothly rotates the character towards the target rotation using `Quaternion.Slerp()`.

### HandleAimedRotation()
- Handles the rotation of the character while aiming.
- Calculates the target rotation based on the camera's forward direction.
- Smoothly rotates the character towards the target rotation using `Quaternion.Slerp()`.

### HandleAnimation()
- Handles the animation of the character based on movement and aiming inputs.
- Updates the animation bools (`isWalking` and `isRunning`) based on the movement and aiming status.

### OnMovementInput(InputAction.CallbackContext ctx)
- Event handler for movement input.
- Updates the `currentMovementInput` vector based on the input value.
- Sets the currentAimedMovement vector to the current movement input.
- Calculates the raw movement vectors (currentMovementRaw and currentRunMovementRaw) based on the input, taking into account the walking or aiming speed.
- Updates the isMovementPressed flag based on whether the movement input is non-zero.

### RotationToVector(float degrees)
- Converts a rotation in degrees to a normalized vector.
- Creates a quaternion rotation based on the given degrees.
- Multiplies the rotation with the downward vector to get the resulting vector.

### OnEnable()
- Enables the player input by enabling the Movement action map.

### OnDisable()
- Disables the player input by disabling the Movement action map.

This script provides the functionality to control a player character's movement, rotation, and animation based on input. It utilizes the Unity CharacterController component for movement and the Animator component for animation. The script handles both regular movement and movement while aiming, with different speeds and rotation behaviors. It also responds to run input for increased movement speed.

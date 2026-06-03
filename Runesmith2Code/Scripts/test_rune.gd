extends Node2D

@onready var visuals: SpineSprite = %Visuals

func _ready() -> void:
	var data = visuals.get_skeleton().get_data()
	
	var anim = visuals.get_animation_state()
	anim.set_animation("idle_loop", true, 0)


func _input(event: InputEvent) -> void:
	if event is InputEventMouseButton:
		if event.is_released() and event.button_index == MOUSE_BUTTON_LEFT:
			var anim = visuals.get_animation_state()
			var track = anim.get_current(0)
			var time = track.get_track_time()
			track = anim.set_animation("trigger", false, 0)
			track = anim.set_animation("idle_loop", true, 0)
			track.set_track_time(time)

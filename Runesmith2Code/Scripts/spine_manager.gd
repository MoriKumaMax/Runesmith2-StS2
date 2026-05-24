@tool
extends SpineSprite

@export var add_anim_mix_entry: bool = false:
	set(value):
		if add_anim_mix_entry == value: return
		add_animation_mix_entry()
	
func add_animation_mix_entry() -> void:
	#This exists as a temporary workaround for the Spine GDExtension glitch that
	#causes Godot to crash when trying to add a new Animation Mix entry in the
	#inspector. Can be removed once Esoteric fixes this glitch.

	if !Engine.is_editor_hint(): return
	var skeleton_file_res: SpineSkeletonFileResource = skeleton_data_res.skeleton_file_res
	skeleton_data_res.skeleton_file_res = null
	skeleton_data_res.animation_mixes.append(SpineAnimationMix.new())
	skeleton_data_res.skeleton_file_res = skeleton_file_res

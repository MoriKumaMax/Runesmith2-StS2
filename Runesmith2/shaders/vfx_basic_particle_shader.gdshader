shader_type canvas_item;
render_mode blend_mix;

#include "res://Runesmith2/shaders/flipbook.gdshaderinc"

uniform float frame_count = 4;
uniform vec2 flipbook_size = vec2(2,2);


uniform vec2 pivot_offset = vec2(0.0, 0.0);

varying float lifetime;
varying float flipbook_offset;
varying vec4 vertex_color;

void vertex() {
	vertex_color = COLOR;
	lifetime = INSTANCE_CUSTOM.y/INSTANCE_CUSTOM.w;
	flipbook_offset = INSTANCE_CUSTOM.z;
	
	VERTEX.x *= TEXTURE_PIXEL_SIZE.x/TEXTURE_PIXEL_SIZE.y;
	VERTEX += pivot_offset * (1.0/TEXTURE_PIXEL_SIZE);
}

void fragment() {
	vec2 flipbook_uv = flipbook(UV, flipbook_size, frame_count * flipbook_offset * 0.99);
	
	vec4 texture_color =  texture(TEXTURE, flipbook_uv);

	COLOR = texture_color * vertex_color;
}
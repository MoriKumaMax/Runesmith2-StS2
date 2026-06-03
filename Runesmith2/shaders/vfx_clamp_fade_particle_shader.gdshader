shader_type canvas_item;
render_mode blend_mix;

#include "res://Runesmith2/shaders/flipbook.gdshaderinc"

uniform float frame_count = 4;
uniform vec2 flipbook_size = vec2(2,2);

uniform sampler2D lut;

uniform vec2 pivot_offset = vec2(0.0, 0.0);

uniform sampler2D erosion_btm_curve;
uniform sampler2D erosion_curve;
uniform float erosion_offset = 0.25;

uniform sampler2D noise: filter_nearest, repeat_enable;
uniform vec2 noise_offset_scale = vec2(0.1, 0.1);

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
	vec2 noise_offset = (texture(noise, vec2(lifetime * 0.3) + UV).rg * noise_offset_scale) - noise_offset_scale * 0.5;
	
	vec2 flipbook_uv = flipbook(UV, flipbook_size, frame_count * flipbook_offset * 0.99);
	
	vec4 texture_color =  texture(TEXTURE, flipbook_uv + noise_offset);

	vec4 lut_color = texture(lut, texture_color.rr);

	float erosion_curve_val = texture(erosion_curve, vec2(lifetime)).r;
	float erosion_val = smoothstep(erosion_curve_val, erosion_curve_val + erosion_offset, texture_color.a);
	float erosion_curve_val2 = texture(erosion_btm_curve, vec2(lifetime)).r;
	float erosion_val2 = smoothstep(erosion_curve_val2, erosion_curve_val2 + erosion_offset, texture_color.a);
	float alpha = erosion_val - erosion_val2;

	COLOR = vec4(lut_color.rgb, alpha * lut_color.a) * vertex_color;
}
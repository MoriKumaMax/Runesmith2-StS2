shader_type canvas_item;
render_mode blend_mix;

#include "res://Runesmith2/shaders/flipbook.gdshaderinc"

uniform vec2 tex_scroll_speed = vec2(0);

uniform float lut_scroll_speed = 1;
uniform sampler2D lut: repeat_enable;

uniform vec2 pivot_offset = vec2(0.0, 0.0);

uniform sampler2D erosion_curve;
uniform float erosion_offset = 0.25;

uniform sampler2D noise: filter_nearest, repeat_enable;
uniform vec2 noise_offset_scale = vec2(0.1, 0.1);

uniform sampler2D mask;

varying float lifetime;
varying float flipbook_offset;
varying vec4 vertex_color;

void vertex() {
	vertex_color = COLOR;
	lifetime = INSTANCE_CUSTOM.y/INSTANCE_CUSTOM.w;
}

void fragment() {
	vec2 noise_offset = (texture(noise, vec2(-lifetime, 0) + UV).rg * noise_offset_scale) - noise_offset_scale * 0.5;
	
	vec4 texture_color =  texture(TEXTURE, UV + noise_offset + lifetime * tex_scroll_speed);

	vec4 lut_color = texture(lut, vec2(texture_color.r + lifetime * lut_scroll_speed));
	
	vec4 mask_color = texture(mask, UV + noise_offset);

	float erosion_curve_val = texture(erosion_curve, vec2(lifetime)).r;
	float erosion_val = smoothstep(erosion_curve_val, erosion_curve_val + erosion_offset, texture_color.a);

	COLOR = vec4(lut_color.rgb, erosion_val * lut_color.a * mask_color.r) * vertex_color;
}
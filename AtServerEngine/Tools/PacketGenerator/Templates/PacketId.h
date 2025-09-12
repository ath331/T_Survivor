#pragma once

enum class EPacketId : unsigned __int16
{
{%- for pkt in parser.packet_id %}
	PKT_{{pkt.name}} = {{pkt.id}},
{%- endfor %}
};

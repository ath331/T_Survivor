#pragma once

enum class EPacketId : unsigned __int16
{
{%- for pkt in parser.total_pkt %}
	PKT_{{pkt.name}} = {{pkt.id}},
{%- endfor %}
};

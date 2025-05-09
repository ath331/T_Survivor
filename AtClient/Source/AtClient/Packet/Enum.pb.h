// Generated by the protocol buffer compiler.  DO NOT EDIT!
// source: Enum.proto

#ifndef GOOGLE_PROTOBUF_INCLUDED_Enum_2eproto
#define GOOGLE_PROTOBUF_INCLUDED_Enum_2eproto

#include <limits>
#include <string>

#include <google/protobuf/port_def.inc>
#if PROTOBUF_VERSION < 3017000
#error This file was generated by a newer version of protoc which is
#error incompatible with your Protocol Buffer headers. Please update
#error your headers.
#endif
#if 3017001 < PROTOBUF_MIN_PROTOC_VERSION
#error This file was generated by an older version of protoc which is
#error incompatible with your Protocol Buffer headers. Please
#error regenerate this file with a newer version of protoc.
#endif

#include <google/protobuf/port_undef.inc>
#include <google/protobuf/io/coded_stream.h>
#include <google/protobuf/arena.h>
#include <google/protobuf/arenastring.h>
#include <google/protobuf/generated_message_table_driven.h>
#include <google/protobuf/generated_message_util.h>
#include <google/protobuf/metadata_lite.h>
#include <google/protobuf/generated_message_reflection.h>
#include <google/protobuf/repeated_field.h>  // IWYU pragma: export
#include <google/protobuf/extension_set.h>  // IWYU pragma: export
#include <google/protobuf/generated_enum_reflection.h>
// @@protoc_insertion_point(includes)
#include <google/protobuf/port_def.inc>
#define PROTOBUF_INTERNAL_EXPORT_Enum_2eproto
PROTOBUF_NAMESPACE_OPEN
namespace internal {
class AnyMetadata;
}  // namespace internal
PROTOBUF_NAMESPACE_CLOSE

// Internal implementation detail -- do not use these members.
struct TableStruct_Enum_2eproto {
  static const ::PROTOBUF_NAMESPACE_ID::internal::ParseTableField entries[]
    PROTOBUF_SECTION_VARIABLE(protodesc_cold);
  static const ::PROTOBUF_NAMESPACE_ID::internal::AuxiliaryParseTableField aux[]
    PROTOBUF_SECTION_VARIABLE(protodesc_cold);
  static const ::PROTOBUF_NAMESPACE_ID::internal::ParseTable schema[1]
    PROTOBUF_SECTION_VARIABLE(protodesc_cold);
  static const ::PROTOBUF_NAMESPACE_ID::internal::FieldMetadata field_metadata[];
  static const ::PROTOBUF_NAMESPACE_ID::internal::SerializationTable serialization_table[];
  static const ::PROTOBUF_NAMESPACE_ID::uint32 offsets[];
};
extern const ::PROTOBUF_NAMESPACE_ID::internal::DescriptorTable descriptor_table_Enum_2eproto;
PROTOBUF_NAMESPACE_OPEN
PROTOBUF_NAMESPACE_CLOSE
namespace Protocol {

enum EResultCode : int {
  RESULT_CODE_SUCCESS = 0,
  RESULT_CODE_FAIL_ROOM_ENTER = 1,
  RESULT_CODE_MAX = 2,
  EResultCode_INT_MIN_SENTINEL_DO_NOT_USE_ = std::numeric_limits<::PROTOBUF_NAMESPACE_ID::int32>::min(),
  EResultCode_INT_MAX_SENTINEL_DO_NOT_USE_ = std::numeric_limits<::PROTOBUF_NAMESPACE_ID::int32>::max()
};
bool EResultCode_IsValid(int value);
constexpr EResultCode EResultCode_MIN = RESULT_CODE_SUCCESS;
constexpr EResultCode EResultCode_MAX = RESULT_CODE_MAX;
constexpr int EResultCode_ARRAYSIZE = EResultCode_MAX + 1;

const ::PROTOBUF_NAMESPACE_ID::EnumDescriptor* EResultCode_descriptor();
template<typename T>
inline const std::string& EResultCode_Name(T enum_t_value) {
  static_assert(::std::is_same<T, EResultCode>::value ||
    ::std::is_integral<T>::value,
    "Incorrect type passed to function EResultCode_Name.");
  return ::PROTOBUF_NAMESPACE_ID::internal::NameOfEnum(
    EResultCode_descriptor(), enum_t_value);
}
inline bool EResultCode_Parse(
    ::PROTOBUF_NAMESPACE_ID::ConstStringParam name, EResultCode* value) {
  return ::PROTOBUF_NAMESPACE_ID::internal::ParseNamedEnum<EResultCode>(
    EResultCode_descriptor(), name, value);
}
enum EObjectType : int {
  OBJECT_TYPE_NONE = 0,
  OBJECT_TYPE_ACTOR = 1,
  OBJECT_TYPE_PROJECTILE = 2,
  OBJECT_TYPE_ENV = 3,
  OBJECT_TYPE_MAX = 4,
  EObjectType_INT_MIN_SENTINEL_DO_NOT_USE_ = std::numeric_limits<::PROTOBUF_NAMESPACE_ID::int32>::min(),
  EObjectType_INT_MAX_SENTINEL_DO_NOT_USE_ = std::numeric_limits<::PROTOBUF_NAMESPACE_ID::int32>::max()
};
bool EObjectType_IsValid(int value);
constexpr EObjectType EObjectType_MIN = OBJECT_TYPE_NONE;
constexpr EObjectType EObjectType_MAX = OBJECT_TYPE_MAX;
constexpr int EObjectType_ARRAYSIZE = EObjectType_MAX + 1;

const ::PROTOBUF_NAMESPACE_ID::EnumDescriptor* EObjectType_descriptor();
template<typename T>
inline const std::string& EObjectType_Name(T enum_t_value) {
  static_assert(::std::is_same<T, EObjectType>::value ||
    ::std::is_integral<T>::value,
    "Incorrect type passed to function EObjectType_Name.");
  return ::PROTOBUF_NAMESPACE_ID::internal::NameOfEnum(
    EObjectType_descriptor(), enum_t_value);
}
inline bool EObjectType_Parse(
    ::PROTOBUF_NAMESPACE_ID::ConstStringParam name, EObjectType* value) {
  return ::PROTOBUF_NAMESPACE_ID::internal::ParseNamedEnum<EObjectType>(
    EObjectType_descriptor(), name, value);
}
enum EActorType : int {
  ACTOR_TYPE_NONE = 0,
  ACTOR_TYPE_PLAYER = 1,
  ACTOR_TYPE_MONSTER = 2,
  ACTOR_TYPE_NPC = 3,
  ACTOR_TYPE_MAX = 4,
  EActorType_INT_MIN_SENTINEL_DO_NOT_USE_ = std::numeric_limits<::PROTOBUF_NAMESPACE_ID::int32>::min(),
  EActorType_INT_MAX_SENTINEL_DO_NOT_USE_ = std::numeric_limits<::PROTOBUF_NAMESPACE_ID::int32>::max()
};
bool EActorType_IsValid(int value);
constexpr EActorType EActorType_MIN = ACTOR_TYPE_NONE;
constexpr EActorType EActorType_MAX = ACTOR_TYPE_MAX;
constexpr int EActorType_ARRAYSIZE = EActorType_MAX + 1;

const ::PROTOBUF_NAMESPACE_ID::EnumDescriptor* EActorType_descriptor();
template<typename T>
inline const std::string& EActorType_Name(T enum_t_value) {
  static_assert(::std::is_same<T, EActorType>::value ||
    ::std::is_integral<T>::value,
    "Incorrect type passed to function EActorType_Name.");
  return ::PROTOBUF_NAMESPACE_ID::internal::NameOfEnum(
    EActorType_descriptor(), enum_t_value);
}
inline bool EActorType_Parse(
    ::PROTOBUF_NAMESPACE_ID::ConstStringParam name, EActorType* value) {
  return ::PROTOBUF_NAMESPACE_ID::internal::ParseNamedEnum<EActorType>(
    EActorType_descriptor(), name, value);
}
enum EBagType : int {
  BAG_TYPE_NONE = 0,
  BAG_TYPE_EQUIPMENT = 1,
  BAG_TYPE_ETC = 2,
  BAG_TYPE_USEABLE = 3,
  BAG_TYPE_MAX = 4,
  EBagType_INT_MIN_SENTINEL_DO_NOT_USE_ = std::numeric_limits<::PROTOBUF_NAMESPACE_ID::int32>::min(),
  EBagType_INT_MAX_SENTINEL_DO_NOT_USE_ = std::numeric_limits<::PROTOBUF_NAMESPACE_ID::int32>::max()
};
bool EBagType_IsValid(int value);
constexpr EBagType EBagType_MIN = BAG_TYPE_NONE;
constexpr EBagType EBagType_MAX = BAG_TYPE_MAX;
constexpr int EBagType_ARRAYSIZE = EBagType_MAX + 1;

const ::PROTOBUF_NAMESPACE_ID::EnumDescriptor* EBagType_descriptor();
template<typename T>
inline const std::string& EBagType_Name(T enum_t_value) {
  static_assert(::std::is_same<T, EBagType>::value ||
    ::std::is_integral<T>::value,
    "Incorrect type passed to function EBagType_Name.");
  return ::PROTOBUF_NAMESPACE_ID::internal::NameOfEnum(
    EBagType_descriptor(), enum_t_value);
}
inline bool EBagType_Parse(
    ::PROTOBUF_NAMESPACE_ID::ConstStringParam name, EBagType* value) {
  return ::PROTOBUF_NAMESPACE_ID::internal::ParseNamedEnum<EBagType>(
    EBagType_descriptor(), name, value);
}
enum EPlayerType : int {
  PLAYER_TYPE_NONE = 0,
  PLAYER_TYPE_KNIGHT = 1,
  PLAYER_TYPE_MAGE = 2,
  PLAYER_TYPE_ARCHER = 3,
  PLAYER_TYPE_MAX = 4,
  EPlayerType_INT_MIN_SENTINEL_DO_NOT_USE_ = std::numeric_limits<::PROTOBUF_NAMESPACE_ID::int32>::min(),
  EPlayerType_INT_MAX_SENTINEL_DO_NOT_USE_ = std::numeric_limits<::PROTOBUF_NAMESPACE_ID::int32>::max()
};
bool EPlayerType_IsValid(int value);
constexpr EPlayerType EPlayerType_MIN = PLAYER_TYPE_NONE;
constexpr EPlayerType EPlayerType_MAX = PLAYER_TYPE_MAX;
constexpr int EPlayerType_ARRAYSIZE = EPlayerType_MAX + 1;

const ::PROTOBUF_NAMESPACE_ID::EnumDescriptor* EPlayerType_descriptor();
template<typename T>
inline const std::string& EPlayerType_Name(T enum_t_value) {
  static_assert(::std::is_same<T, EPlayerType>::value ||
    ::std::is_integral<T>::value,
    "Incorrect type passed to function EPlayerType_Name.");
  return ::PROTOBUF_NAMESPACE_ID::internal::NameOfEnum(
    EPlayerType_descriptor(), enum_t_value);
}
inline bool EPlayerType_Parse(
    ::PROTOBUF_NAMESPACE_ID::ConstStringParam name, EPlayerType* value) {
  return ::PROTOBUF_NAMESPACE_ID::internal::ParseNamedEnum<EPlayerType>(
    EPlayerType_descriptor(), name, value);
}
enum EMoveState : int {
  MOVE_STATE_NONE = 0,
  MOVE_STATE_IDLE = 1,
  MOVE_STATE_RUN = 2,
  MOVE_STATE_JUMP = 3,
  MOVE_STATE_SKILL = 4,
  EMoveState_INT_MIN_SENTINEL_DO_NOT_USE_ = std::numeric_limits<::PROTOBUF_NAMESPACE_ID::int32>::min(),
  EMoveState_INT_MAX_SENTINEL_DO_NOT_USE_ = std::numeric_limits<::PROTOBUF_NAMESPACE_ID::int32>::max()
};
bool EMoveState_IsValid(int value);
constexpr EMoveState EMoveState_MIN = MOVE_STATE_NONE;
constexpr EMoveState EMoveState_MAX = MOVE_STATE_SKILL;
constexpr int EMoveState_ARRAYSIZE = EMoveState_MAX + 1;

const ::PROTOBUF_NAMESPACE_ID::EnumDescriptor* EMoveState_descriptor();
template<typename T>
inline const std::string& EMoveState_Name(T enum_t_value) {
  static_assert(::std::is_same<T, EMoveState>::value ||
    ::std::is_integral<T>::value,
    "Incorrect type passed to function EMoveState_Name.");
  return ::PROTOBUF_NAMESPACE_ID::internal::NameOfEnum(
    EMoveState_descriptor(), enum_t_value);
}
inline bool EMoveState_Parse(
    ::PROTOBUF_NAMESPACE_ID::ConstStringParam name, EMoveState* value) {
  return ::PROTOBUF_NAMESPACE_ID::internal::ParseNamedEnum<EMoveState>(
    EMoveState_descriptor(), name, value);
}
enum EEquipSlotType : int {
  EQUIP_SLOT_TYPE_NONE = 0,
  EQUIP_SLOT_TYPE_WEAPON = 1,
  EQUIP_SLOT_TYPE_SUB_WEAPON = 2,
  EQUIP_SLOT_TYPE_HELMAT = 3,
  EQUIP_SLOT_TYPE_ARMOR = 4,
  EQUIP_SLOT_TYPE_GLOVES = 5,
  EQUIP_SLOT_TYPE_BOOTS = 6,
  EQUIP_SLOT_TYPE_MAX = 7,
  EEquipSlotType_INT_MIN_SENTINEL_DO_NOT_USE_ = std::numeric_limits<::PROTOBUF_NAMESPACE_ID::int32>::min(),
  EEquipSlotType_INT_MAX_SENTINEL_DO_NOT_USE_ = std::numeric_limits<::PROTOBUF_NAMESPACE_ID::int32>::max()
};
bool EEquipSlotType_IsValid(int value);
constexpr EEquipSlotType EEquipSlotType_MIN = EQUIP_SLOT_TYPE_NONE;
constexpr EEquipSlotType EEquipSlotType_MAX = EQUIP_SLOT_TYPE_MAX;
constexpr int EEquipSlotType_ARRAYSIZE = EEquipSlotType_MAX + 1;

const ::PROTOBUF_NAMESPACE_ID::EnumDescriptor* EEquipSlotType_descriptor();
template<typename T>
inline const std::string& EEquipSlotType_Name(T enum_t_value) {
  static_assert(::std::is_same<T, EEquipSlotType>::value ||
    ::std::is_integral<T>::value,
    "Incorrect type passed to function EEquipSlotType_Name.");
  return ::PROTOBUF_NAMESPACE_ID::internal::NameOfEnum(
    EEquipSlotType_descriptor(), enum_t_value);
}
inline bool EEquipSlotType_Parse(
    ::PROTOBUF_NAMESPACE_ID::ConstStringParam name, EEquipSlotType* value) {
  return ::PROTOBUF_NAMESPACE_ID::internal::ParseNamedEnum<EEquipSlotType>(
    EEquipSlotType_descriptor(), name, value);
}
enum EStat : int {
  STAT_NONE = 0,
  STAT_STRENGTH = 1,
  STAT_HP = 2,
  STAT_MP = 3,
  STAT_DEFENSE = 4,
  STAT_SPEED = 5,
  STAT_INTELLIGENCE = 6,
  STAT_MAX = 7,
  EStat_INT_MIN_SENTINEL_DO_NOT_USE_ = std::numeric_limits<::PROTOBUF_NAMESPACE_ID::int32>::min(),
  EStat_INT_MAX_SENTINEL_DO_NOT_USE_ = std::numeric_limits<::PROTOBUF_NAMESPACE_ID::int32>::max()
};
bool EStat_IsValid(int value);
constexpr EStat EStat_MIN = STAT_NONE;
constexpr EStat EStat_MAX = STAT_MAX;
constexpr int EStat_ARRAYSIZE = EStat_MAX + 1;

const ::PROTOBUF_NAMESPACE_ID::EnumDescriptor* EStat_descriptor();
template<typename T>
inline const std::string& EStat_Name(T enum_t_value) {
  static_assert(::std::is_same<T, EStat>::value ||
    ::std::is_integral<T>::value,
    "Incorrect type passed to function EStat_Name.");
  return ::PROTOBUF_NAMESPACE_ID::internal::NameOfEnum(
    EStat_descriptor(), enum_t_value);
}
inline bool EStat_Parse(
    ::PROTOBUF_NAMESPACE_ID::ConstStringParam name, EStat* value) {
  return ::PROTOBUF_NAMESPACE_ID::internal::ParseNamedEnum<EStat>(
    EStat_descriptor(), name, value);
}
enum EAnimationParamType : int {
  ANIM_PARAM_TYPE_BOOL = 0,
  ANIM_PARAM_TYPE_FLOAT = 1,
  ANIM_PARAM_TYPE_TRIGGER = 2,
  EAnimationParamType_INT_MIN_SENTINEL_DO_NOT_USE_ = std::numeric_limits<::PROTOBUF_NAMESPACE_ID::int32>::min(),
  EAnimationParamType_INT_MAX_SENTINEL_DO_NOT_USE_ = std::numeric_limits<::PROTOBUF_NAMESPACE_ID::int32>::max()
};
bool EAnimationParamType_IsValid(int value);
constexpr EAnimationParamType EAnimationParamType_MIN = ANIM_PARAM_TYPE_BOOL;
constexpr EAnimationParamType EAnimationParamType_MAX = ANIM_PARAM_TYPE_TRIGGER;
constexpr int EAnimationParamType_ARRAYSIZE = EAnimationParamType_MAX + 1;

const ::PROTOBUF_NAMESPACE_ID::EnumDescriptor* EAnimationParamType_descriptor();
template<typename T>
inline const std::string& EAnimationParamType_Name(T enum_t_value) {
  static_assert(::std::is_same<T, EAnimationParamType>::value ||
    ::std::is_integral<T>::value,
    "Incorrect type passed to function EAnimationParamType_Name.");
  return ::PROTOBUF_NAMESPACE_ID::internal::NameOfEnum(
    EAnimationParamType_descriptor(), enum_t_value);
}
inline bool EAnimationParamType_Parse(
    ::PROTOBUF_NAMESPACE_ID::ConstStringParam name, EAnimationParamType* value) {
  return ::PROTOBUF_NAMESPACE_ID::internal::ParseNamedEnum<EAnimationParamType>(
    EAnimationParamType_descriptor(), name, value);
}
// ===================================================================


// ===================================================================


// ===================================================================

#ifdef __GNUC__
  #pragma GCC diagnostic push
  #pragma GCC diagnostic ignored "-Wstrict-aliasing"
#endif  // __GNUC__
#ifdef __GNUC__
  #pragma GCC diagnostic pop
#endif  // __GNUC__

// @@protoc_insertion_point(namespace_scope)

}  // namespace Protocol

PROTOBUF_NAMESPACE_OPEN

template <> struct is_proto_enum< ::Protocol::EResultCode> : ::std::true_type {};
template <>
inline const EnumDescriptor* GetEnumDescriptor< ::Protocol::EResultCode>() {
  return ::Protocol::EResultCode_descriptor();
}
template <> struct is_proto_enum< ::Protocol::EObjectType> : ::std::true_type {};
template <>
inline const EnumDescriptor* GetEnumDescriptor< ::Protocol::EObjectType>() {
  return ::Protocol::EObjectType_descriptor();
}
template <> struct is_proto_enum< ::Protocol::EActorType> : ::std::true_type {};
template <>
inline const EnumDescriptor* GetEnumDescriptor< ::Protocol::EActorType>() {
  return ::Protocol::EActorType_descriptor();
}
template <> struct is_proto_enum< ::Protocol::EBagType> : ::std::true_type {};
template <>
inline const EnumDescriptor* GetEnumDescriptor< ::Protocol::EBagType>() {
  return ::Protocol::EBagType_descriptor();
}
template <> struct is_proto_enum< ::Protocol::EPlayerType> : ::std::true_type {};
template <>
inline const EnumDescriptor* GetEnumDescriptor< ::Protocol::EPlayerType>() {
  return ::Protocol::EPlayerType_descriptor();
}
template <> struct is_proto_enum< ::Protocol::EMoveState> : ::std::true_type {};
template <>
inline const EnumDescriptor* GetEnumDescriptor< ::Protocol::EMoveState>() {
  return ::Protocol::EMoveState_descriptor();
}
template <> struct is_proto_enum< ::Protocol::EEquipSlotType> : ::std::true_type {};
template <>
inline const EnumDescriptor* GetEnumDescriptor< ::Protocol::EEquipSlotType>() {
  return ::Protocol::EEquipSlotType_descriptor();
}
template <> struct is_proto_enum< ::Protocol::EStat> : ::std::true_type {};
template <>
inline const EnumDescriptor* GetEnumDescriptor< ::Protocol::EStat>() {
  return ::Protocol::EStat_descriptor();
}
template <> struct is_proto_enum< ::Protocol::EAnimationParamType> : ::std::true_type {};
template <>
inline const EnumDescriptor* GetEnumDescriptor< ::Protocol::EAnimationParamType>() {
  return ::Protocol::EAnimationParamType_descriptor();
}

PROTOBUF_NAMESPACE_CLOSE

// @@protoc_insertion_point(global_scope)

#include <google/protobuf/port_undef.inc>
#endif  // GOOGLE_PROTOBUF_INCLUDED_GOOGLE_PROTOBUF_INCLUDED_Enum_2eproto

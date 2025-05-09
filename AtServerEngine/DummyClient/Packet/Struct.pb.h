// Generated by the protocol buffer compiler.  DO NOT EDIT!
// source: Struct.proto

#ifndef GOOGLE_PROTOBUF_INCLUDED_Struct_2eproto
#define GOOGLE_PROTOBUF_INCLUDED_Struct_2eproto

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
#include <google/protobuf/message.h>
#include <google/protobuf/repeated_field.h>  // IWYU pragma: export
#include <google/protobuf/extension_set.h>  // IWYU pragma: export
#include <google/protobuf/unknown_field_set.h>
#include "Enum.pb.h"
// @@protoc_insertion_point(includes)
#include <google/protobuf/port_def.inc>
#define PROTOBUF_INTERNAL_EXPORT_Struct_2eproto
PROTOBUF_NAMESPACE_OPEN
namespace internal {
class AnyMetadata;
}  // namespace internal
PROTOBUF_NAMESPACE_CLOSE

// Internal implementation detail -- do not use these members.
struct TableStruct_Struct_2eproto {
  static const ::PROTOBUF_NAMESPACE_ID::internal::ParseTableField entries[]
    PROTOBUF_SECTION_VARIABLE(protodesc_cold);
  static const ::PROTOBUF_NAMESPACE_ID::internal::AuxiliaryParseTableField aux[]
    PROTOBUF_SECTION_VARIABLE(protodesc_cold);
  static const ::PROTOBUF_NAMESPACE_ID::internal::ParseTable schema[2]
    PROTOBUF_SECTION_VARIABLE(protodesc_cold);
  static const ::PROTOBUF_NAMESPACE_ID::internal::FieldMetadata field_metadata[];
  static const ::PROTOBUF_NAMESPACE_ID::internal::SerializationTable serialization_table[];
  static const ::PROTOBUF_NAMESPACE_ID::uint32 offsets[];
};
extern const ::PROTOBUF_NAMESPACE_ID::internal::DescriptorTable descriptor_table_Struct_2eproto;
namespace Protocol {
class ObjectInfo;
struct ObjectInfoDefaultTypeInternal;
extern ObjectInfoDefaultTypeInternal _ObjectInfo_default_instance_;
class PosInfo;
struct PosInfoDefaultTypeInternal;
extern PosInfoDefaultTypeInternal _PosInfo_default_instance_;
}  // namespace Protocol
PROTOBUF_NAMESPACE_OPEN
template<> ::Protocol::ObjectInfo* Arena::CreateMaybeMessage<::Protocol::ObjectInfo>(Arena*);
template<> ::Protocol::PosInfo* Arena::CreateMaybeMessage<::Protocol::PosInfo>(Arena*);
PROTOBUF_NAMESPACE_CLOSE
namespace Protocol {

// ===================================================================

class PosInfo final :
    public ::PROTOBUF_NAMESPACE_ID::Message /* @@protoc_insertion_point(class_definition:Protocol.PosInfo) */ {
 public:
  inline PosInfo() : PosInfo(nullptr) {}
  ~PosInfo() override;
  explicit constexpr PosInfo(::PROTOBUF_NAMESPACE_ID::internal::ConstantInitialized);

  PosInfo(const PosInfo& from);
  PosInfo(PosInfo&& from) noexcept
    : PosInfo() {
    *this = ::std::move(from);
  }

  inline PosInfo& operator=(const PosInfo& from) {
    CopyFrom(from);
    return *this;
  }
  inline PosInfo& operator=(PosInfo&& from) noexcept {
    if (this == &from) return *this;
    if (GetOwningArena() == from.GetOwningArena()) {
      InternalSwap(&from);
    } else {
      CopyFrom(from);
    }
    return *this;
  }

  static const ::PROTOBUF_NAMESPACE_ID::Descriptor* descriptor() {
    return GetDescriptor();
  }
  static const ::PROTOBUF_NAMESPACE_ID::Descriptor* GetDescriptor() {
    return default_instance().GetMetadata().descriptor;
  }
  static const ::PROTOBUF_NAMESPACE_ID::Reflection* GetReflection() {
    return default_instance().GetMetadata().reflection;
  }
  static const PosInfo& default_instance() {
    return *internal_default_instance();
  }
  static inline const PosInfo* internal_default_instance() {
    return reinterpret_cast<const PosInfo*>(
               &_PosInfo_default_instance_);
  }
  static constexpr int kIndexInFileMessages =
    0;

  friend void swap(PosInfo& a, PosInfo& b) {
    a.Swap(&b);
  }
  inline void Swap(PosInfo* other) {
    if (other == this) return;
    if (GetOwningArena() == other->GetOwningArena()) {
      InternalSwap(other);
    } else {
      ::PROTOBUF_NAMESPACE_ID::internal::GenericSwap(this, other);
    }
  }
  void UnsafeArenaSwap(PosInfo* other) {
    if (other == this) return;
    GOOGLE_DCHECK(GetOwningArena() == other->GetOwningArena());
    InternalSwap(other);
  }

  // implements Message ----------------------------------------------

  inline PosInfo* New() const final {
    return new PosInfo();
  }

  PosInfo* New(::PROTOBUF_NAMESPACE_ID::Arena* arena) const final {
    return CreateMaybeMessage<PosInfo>(arena);
  }
  void CopyFrom(const ::PROTOBUF_NAMESPACE_ID::Message& from) final;
  void MergeFrom(const ::PROTOBUF_NAMESPACE_ID::Message& from) final;
  void CopyFrom(const PosInfo& from);
  void MergeFrom(const PosInfo& from);
  PROTOBUF_ATTRIBUTE_REINITIALIZES void Clear() final;
  bool IsInitialized() const final;

  size_t ByteSizeLong() const final;
  const char* _InternalParse(const char* ptr, ::PROTOBUF_NAMESPACE_ID::internal::ParseContext* ctx) final;
  ::PROTOBUF_NAMESPACE_ID::uint8* _InternalSerialize(
      ::PROTOBUF_NAMESPACE_ID::uint8* target, ::PROTOBUF_NAMESPACE_ID::io::EpsCopyOutputStream* stream) const final;
  int GetCachedSize() const final { return _cached_size_.Get(); }

  private:
  void SharedCtor();
  void SharedDtor();
  void SetCachedSize(int size) const final;
  void InternalSwap(PosInfo* other);
  friend class ::PROTOBUF_NAMESPACE_ID::internal::AnyMetadata;
  static ::PROTOBUF_NAMESPACE_ID::StringPiece FullMessageName() {
    return "Protocol.PosInfo";
  }
  protected:
  explicit PosInfo(::PROTOBUF_NAMESPACE_ID::Arena* arena);
  private:
  static void ArenaDtor(void* object);
  inline void RegisterArenaDtor(::PROTOBUF_NAMESPACE_ID::Arena* arena);
  public:

  ::PROTOBUF_NAMESPACE_ID::Metadata GetMetadata() const final;

  // nested types ----------------------------------------------------

  // accessors -------------------------------------------------------

  enum : int {
    kIdFieldNumber = 1,
    kXFieldNumber = 2,
    kYFieldNumber = 3,
    kZFieldNumber = 4,
    kYawFieldNumber = 5,
    kMoveStateFieldNumber = 6,
  };
  // uint64 id = 1;
  void clear_id();
  ::PROTOBUF_NAMESPACE_ID::uint64 id() const;
  void set_id(::PROTOBUF_NAMESPACE_ID::uint64 value);
  private:
  ::PROTOBUF_NAMESPACE_ID::uint64 _internal_id() const;
  void _internal_set_id(::PROTOBUF_NAMESPACE_ID::uint64 value);
  public:

  // float x = 2;
  void clear_x();
  float x() const;
  void set_x(float value);
  private:
  float _internal_x() const;
  void _internal_set_x(float value);
  public:

  // float y = 3;
  void clear_y();
  float y() const;
  void set_y(float value);
  private:
  float _internal_y() const;
  void _internal_set_y(float value);
  public:

  // float z = 4;
  void clear_z();
  float z() const;
  void set_z(float value);
  private:
  float _internal_z() const;
  void _internal_set_z(float value);
  public:

  // float yaw = 5;
  void clear_yaw();
  float yaw() const;
  void set_yaw(float value);
  private:
  float _internal_yaw() const;
  void _internal_set_yaw(float value);
  public:

  // .Protocol.EMoveState move_State = 6;
  void clear_move_state();
  ::Protocol::EMoveState move_state() const;
  void set_move_state(::Protocol::EMoveState value);
  private:
  ::Protocol::EMoveState _internal_move_state() const;
  void _internal_set_move_state(::Protocol::EMoveState value);
  public:

  // @@protoc_insertion_point(class_scope:Protocol.PosInfo)
 private:
  class _Internal;

  template <typename T> friend class ::PROTOBUF_NAMESPACE_ID::Arena::InternalHelper;
  typedef void InternalArenaConstructable_;
  typedef void DestructorSkippable_;
  ::PROTOBUF_NAMESPACE_ID::uint64 id_;
  float x_;
  float y_;
  float z_;
  float yaw_;
  int move_state_;
  mutable ::PROTOBUF_NAMESPACE_ID::internal::CachedSize _cached_size_;
  friend struct ::TableStruct_Struct_2eproto;
};
// -------------------------------------------------------------------

class ObjectInfo final :
    public ::PROTOBUF_NAMESPACE_ID::Message /* @@protoc_insertion_point(class_definition:Protocol.ObjectInfo) */ {
 public:
  inline ObjectInfo() : ObjectInfo(nullptr) {}
  ~ObjectInfo() override;
  explicit constexpr ObjectInfo(::PROTOBUF_NAMESPACE_ID::internal::ConstantInitialized);

  ObjectInfo(const ObjectInfo& from);
  ObjectInfo(ObjectInfo&& from) noexcept
    : ObjectInfo() {
    *this = ::std::move(from);
  }

  inline ObjectInfo& operator=(const ObjectInfo& from) {
    CopyFrom(from);
    return *this;
  }
  inline ObjectInfo& operator=(ObjectInfo&& from) noexcept {
    if (this == &from) return *this;
    if (GetOwningArena() == from.GetOwningArena()) {
      InternalSwap(&from);
    } else {
      CopyFrom(from);
    }
    return *this;
  }

  static const ::PROTOBUF_NAMESPACE_ID::Descriptor* descriptor() {
    return GetDescriptor();
  }
  static const ::PROTOBUF_NAMESPACE_ID::Descriptor* GetDescriptor() {
    return default_instance().GetMetadata().descriptor;
  }
  static const ::PROTOBUF_NAMESPACE_ID::Reflection* GetReflection() {
    return default_instance().GetMetadata().reflection;
  }
  static const ObjectInfo& default_instance() {
    return *internal_default_instance();
  }
  static inline const ObjectInfo* internal_default_instance() {
    return reinterpret_cast<const ObjectInfo*>(
               &_ObjectInfo_default_instance_);
  }
  static constexpr int kIndexInFileMessages =
    1;

  friend void swap(ObjectInfo& a, ObjectInfo& b) {
    a.Swap(&b);
  }
  inline void Swap(ObjectInfo* other) {
    if (other == this) return;
    if (GetOwningArena() == other->GetOwningArena()) {
      InternalSwap(other);
    } else {
      ::PROTOBUF_NAMESPACE_ID::internal::GenericSwap(this, other);
    }
  }
  void UnsafeArenaSwap(ObjectInfo* other) {
    if (other == this) return;
    GOOGLE_DCHECK(GetOwningArena() == other->GetOwningArena());
    InternalSwap(other);
  }

  // implements Message ----------------------------------------------

  inline ObjectInfo* New() const final {
    return new ObjectInfo();
  }

  ObjectInfo* New(::PROTOBUF_NAMESPACE_ID::Arena* arena) const final {
    return CreateMaybeMessage<ObjectInfo>(arena);
  }
  void CopyFrom(const ::PROTOBUF_NAMESPACE_ID::Message& from) final;
  void MergeFrom(const ::PROTOBUF_NAMESPACE_ID::Message& from) final;
  void CopyFrom(const ObjectInfo& from);
  void MergeFrom(const ObjectInfo& from);
  PROTOBUF_ATTRIBUTE_REINITIALIZES void Clear() final;
  bool IsInitialized() const final;

  size_t ByteSizeLong() const final;
  const char* _InternalParse(const char* ptr, ::PROTOBUF_NAMESPACE_ID::internal::ParseContext* ctx) final;
  ::PROTOBUF_NAMESPACE_ID::uint8* _InternalSerialize(
      ::PROTOBUF_NAMESPACE_ID::uint8* target, ::PROTOBUF_NAMESPACE_ID::io::EpsCopyOutputStream* stream) const final;
  int GetCachedSize() const final { return _cached_size_.Get(); }

  private:
  void SharedCtor();
  void SharedDtor();
  void SetCachedSize(int size) const final;
  void InternalSwap(ObjectInfo* other);
  friend class ::PROTOBUF_NAMESPACE_ID::internal::AnyMetadata;
  static ::PROTOBUF_NAMESPACE_ID::StringPiece FullMessageName() {
    return "Protocol.ObjectInfo";
  }
  protected:
  explicit ObjectInfo(::PROTOBUF_NAMESPACE_ID::Arena* arena);
  private:
  static void ArenaDtor(void* object);
  inline void RegisterArenaDtor(::PROTOBUF_NAMESPACE_ID::Arena* arena);
  public:

  ::PROTOBUF_NAMESPACE_ID::Metadata GetMetadata() const final;

  // nested types ----------------------------------------------------

  // accessors -------------------------------------------------------

  enum : int {
    kPosInfoFieldNumber = 5,
    kIdFieldNumber = 1,
    kInfoIdFieldNumber = 2,
    kObjectTypeFieldNumber = 3,
    kActorTypeFieldNumber = 4,
  };
  // .Protocol.PosInfo pos_info = 5;
  bool has_pos_info() const;
  private:
  bool _internal_has_pos_info() const;
  public:
  void clear_pos_info();
  const ::Protocol::PosInfo& pos_info() const;
  PROTOBUF_FUTURE_MUST_USE_RESULT ::Protocol::PosInfo* release_pos_info();
  ::Protocol::PosInfo* mutable_pos_info();
  void set_allocated_pos_info(::Protocol::PosInfo* pos_info);
  private:
  const ::Protocol::PosInfo& _internal_pos_info() const;
  ::Protocol::PosInfo* _internal_mutable_pos_info();
  public:
  void unsafe_arena_set_allocated_pos_info(
      ::Protocol::PosInfo* pos_info);
  ::Protocol::PosInfo* unsafe_arena_release_pos_info();

  // uint64 id = 1;
  void clear_id();
  ::PROTOBUF_NAMESPACE_ID::uint64 id() const;
  void set_id(::PROTOBUF_NAMESPACE_ID::uint64 value);
  private:
  ::PROTOBUF_NAMESPACE_ID::uint64 _internal_id() const;
  void _internal_set_id(::PROTOBUF_NAMESPACE_ID::uint64 value);
  public:

  // int32 infoId = 2;
  void clear_infoid();
  ::PROTOBUF_NAMESPACE_ID::int32 infoid() const;
  void set_infoid(::PROTOBUF_NAMESPACE_ID::int32 value);
  private:
  ::PROTOBUF_NAMESPACE_ID::int32 _internal_infoid() const;
  void _internal_set_infoid(::PROTOBUF_NAMESPACE_ID::int32 value);
  public:

  // .Protocol.EObjectType object_type = 3;
  void clear_object_type();
  ::Protocol::EObjectType object_type() const;
  void set_object_type(::Protocol::EObjectType value);
  private:
  ::Protocol::EObjectType _internal_object_type() const;
  void _internal_set_object_type(::Protocol::EObjectType value);
  public:

  // .Protocol.EActorType actor_type = 4;
  void clear_actor_type();
  ::Protocol::EActorType actor_type() const;
  void set_actor_type(::Protocol::EActorType value);
  private:
  ::Protocol::EActorType _internal_actor_type() const;
  void _internal_set_actor_type(::Protocol::EActorType value);
  public:

  // @@protoc_insertion_point(class_scope:Protocol.ObjectInfo)
 private:
  class _Internal;

  template <typename T> friend class ::PROTOBUF_NAMESPACE_ID::Arena::InternalHelper;
  typedef void InternalArenaConstructable_;
  typedef void DestructorSkippable_;
  ::Protocol::PosInfo* pos_info_;
  ::PROTOBUF_NAMESPACE_ID::uint64 id_;
  ::PROTOBUF_NAMESPACE_ID::int32 infoid_;
  int object_type_;
  int actor_type_;
  mutable ::PROTOBUF_NAMESPACE_ID::internal::CachedSize _cached_size_;
  friend struct ::TableStruct_Struct_2eproto;
};
// ===================================================================


// ===================================================================

#ifdef __GNUC__
  #pragma GCC diagnostic push
  #pragma GCC diagnostic ignored "-Wstrict-aliasing"
#endif  // __GNUC__
// PosInfo

// uint64 id = 1;
inline void PosInfo::clear_id() {
  id_ = uint64_t{0u};
}
inline ::PROTOBUF_NAMESPACE_ID::uint64 PosInfo::_internal_id() const {
  return id_;
}
inline ::PROTOBUF_NAMESPACE_ID::uint64 PosInfo::id() const {
  // @@protoc_insertion_point(field_get:Protocol.PosInfo.id)
  return _internal_id();
}
inline void PosInfo::_internal_set_id(::PROTOBUF_NAMESPACE_ID::uint64 value) {
  
  id_ = value;
}
inline void PosInfo::set_id(::PROTOBUF_NAMESPACE_ID::uint64 value) {
  _internal_set_id(value);
  // @@protoc_insertion_point(field_set:Protocol.PosInfo.id)
}

// float x = 2;
inline void PosInfo::clear_x() {
  x_ = 0;
}
inline float PosInfo::_internal_x() const {
  return x_;
}
inline float PosInfo::x() const {
  // @@protoc_insertion_point(field_get:Protocol.PosInfo.x)
  return _internal_x();
}
inline void PosInfo::_internal_set_x(float value) {
  
  x_ = value;
}
inline void PosInfo::set_x(float value) {
  _internal_set_x(value);
  // @@protoc_insertion_point(field_set:Protocol.PosInfo.x)
}

// float y = 3;
inline void PosInfo::clear_y() {
  y_ = 0;
}
inline float PosInfo::_internal_y() const {
  return y_;
}
inline float PosInfo::y() const {
  // @@protoc_insertion_point(field_get:Protocol.PosInfo.y)
  return _internal_y();
}
inline void PosInfo::_internal_set_y(float value) {
  
  y_ = value;
}
inline void PosInfo::set_y(float value) {
  _internal_set_y(value);
  // @@protoc_insertion_point(field_set:Protocol.PosInfo.y)
}

// float z = 4;
inline void PosInfo::clear_z() {
  z_ = 0;
}
inline float PosInfo::_internal_z() const {
  return z_;
}
inline float PosInfo::z() const {
  // @@protoc_insertion_point(field_get:Protocol.PosInfo.z)
  return _internal_z();
}
inline void PosInfo::_internal_set_z(float value) {
  
  z_ = value;
}
inline void PosInfo::set_z(float value) {
  _internal_set_z(value);
  // @@protoc_insertion_point(field_set:Protocol.PosInfo.z)
}

// float yaw = 5;
inline void PosInfo::clear_yaw() {
  yaw_ = 0;
}
inline float PosInfo::_internal_yaw() const {
  return yaw_;
}
inline float PosInfo::yaw() const {
  // @@protoc_insertion_point(field_get:Protocol.PosInfo.yaw)
  return _internal_yaw();
}
inline void PosInfo::_internal_set_yaw(float value) {
  
  yaw_ = value;
}
inline void PosInfo::set_yaw(float value) {
  _internal_set_yaw(value);
  // @@protoc_insertion_point(field_set:Protocol.PosInfo.yaw)
}

// .Protocol.EMoveState move_State = 6;
inline void PosInfo::clear_move_state() {
  move_state_ = 0;
}
inline ::Protocol::EMoveState PosInfo::_internal_move_state() const {
  return static_cast< ::Protocol::EMoveState >(move_state_);
}
inline ::Protocol::EMoveState PosInfo::move_state() const {
  // @@protoc_insertion_point(field_get:Protocol.PosInfo.move_State)
  return _internal_move_state();
}
inline void PosInfo::_internal_set_move_state(::Protocol::EMoveState value) {
  
  move_state_ = value;
}
inline void PosInfo::set_move_state(::Protocol::EMoveState value) {
  _internal_set_move_state(value);
  // @@protoc_insertion_point(field_set:Protocol.PosInfo.move_State)
}

// -------------------------------------------------------------------

// ObjectInfo

// uint64 id = 1;
inline void ObjectInfo::clear_id() {
  id_ = uint64_t{0u};
}
inline ::PROTOBUF_NAMESPACE_ID::uint64 ObjectInfo::_internal_id() const {
  return id_;
}
inline ::PROTOBUF_NAMESPACE_ID::uint64 ObjectInfo::id() const {
  // @@protoc_insertion_point(field_get:Protocol.ObjectInfo.id)
  return _internal_id();
}
inline void ObjectInfo::_internal_set_id(::PROTOBUF_NAMESPACE_ID::uint64 value) {
  
  id_ = value;
}
inline void ObjectInfo::set_id(::PROTOBUF_NAMESPACE_ID::uint64 value) {
  _internal_set_id(value);
  // @@protoc_insertion_point(field_set:Protocol.ObjectInfo.id)
}

// int32 infoId = 2;
inline void ObjectInfo::clear_infoid() {
  infoid_ = 0;
}
inline ::PROTOBUF_NAMESPACE_ID::int32 ObjectInfo::_internal_infoid() const {
  return infoid_;
}
inline ::PROTOBUF_NAMESPACE_ID::int32 ObjectInfo::infoid() const {
  // @@protoc_insertion_point(field_get:Protocol.ObjectInfo.infoId)
  return _internal_infoid();
}
inline void ObjectInfo::_internal_set_infoid(::PROTOBUF_NAMESPACE_ID::int32 value) {
  
  infoid_ = value;
}
inline void ObjectInfo::set_infoid(::PROTOBUF_NAMESPACE_ID::int32 value) {
  _internal_set_infoid(value);
  // @@protoc_insertion_point(field_set:Protocol.ObjectInfo.infoId)
}

// .Protocol.EObjectType object_type = 3;
inline void ObjectInfo::clear_object_type() {
  object_type_ = 0;
}
inline ::Protocol::EObjectType ObjectInfo::_internal_object_type() const {
  return static_cast< ::Protocol::EObjectType >(object_type_);
}
inline ::Protocol::EObjectType ObjectInfo::object_type() const {
  // @@protoc_insertion_point(field_get:Protocol.ObjectInfo.object_type)
  return _internal_object_type();
}
inline void ObjectInfo::_internal_set_object_type(::Protocol::EObjectType value) {
  
  object_type_ = value;
}
inline void ObjectInfo::set_object_type(::Protocol::EObjectType value) {
  _internal_set_object_type(value);
  // @@protoc_insertion_point(field_set:Protocol.ObjectInfo.object_type)
}

// .Protocol.EActorType actor_type = 4;
inline void ObjectInfo::clear_actor_type() {
  actor_type_ = 0;
}
inline ::Protocol::EActorType ObjectInfo::_internal_actor_type() const {
  return static_cast< ::Protocol::EActorType >(actor_type_);
}
inline ::Protocol::EActorType ObjectInfo::actor_type() const {
  // @@protoc_insertion_point(field_get:Protocol.ObjectInfo.actor_type)
  return _internal_actor_type();
}
inline void ObjectInfo::_internal_set_actor_type(::Protocol::EActorType value) {
  
  actor_type_ = value;
}
inline void ObjectInfo::set_actor_type(::Protocol::EActorType value) {
  _internal_set_actor_type(value);
  // @@protoc_insertion_point(field_set:Protocol.ObjectInfo.actor_type)
}

// .Protocol.PosInfo pos_info = 5;
inline bool ObjectInfo::_internal_has_pos_info() const {
  return this != internal_default_instance() && pos_info_ != nullptr;
}
inline bool ObjectInfo::has_pos_info() const {
  return _internal_has_pos_info();
}
inline void ObjectInfo::clear_pos_info() {
  if (GetArenaForAllocation() == nullptr && pos_info_ != nullptr) {
    delete pos_info_;
  }
  pos_info_ = nullptr;
}
inline const ::Protocol::PosInfo& ObjectInfo::_internal_pos_info() const {
  const ::Protocol::PosInfo* p = pos_info_;
  return p != nullptr ? *p : reinterpret_cast<const ::Protocol::PosInfo&>(
      ::Protocol::_PosInfo_default_instance_);
}
inline const ::Protocol::PosInfo& ObjectInfo::pos_info() const {
  // @@protoc_insertion_point(field_get:Protocol.ObjectInfo.pos_info)
  return _internal_pos_info();
}
inline void ObjectInfo::unsafe_arena_set_allocated_pos_info(
    ::Protocol::PosInfo* pos_info) {
  if (GetArenaForAllocation() == nullptr) {
    delete reinterpret_cast<::PROTOBUF_NAMESPACE_ID::MessageLite*>(pos_info_);
  }
  pos_info_ = pos_info;
  if (pos_info) {
    
  } else {
    
  }
  // @@protoc_insertion_point(field_unsafe_arena_set_allocated:Protocol.ObjectInfo.pos_info)
}
inline ::Protocol::PosInfo* ObjectInfo::release_pos_info() {
  
  ::Protocol::PosInfo* temp = pos_info_;
  pos_info_ = nullptr;
  if (GetArenaForAllocation() != nullptr) {
    temp = ::PROTOBUF_NAMESPACE_ID::internal::DuplicateIfNonNull(temp);
  }
  return temp;
}
inline ::Protocol::PosInfo* ObjectInfo::unsafe_arena_release_pos_info() {
  // @@protoc_insertion_point(field_release:Protocol.ObjectInfo.pos_info)
  
  ::Protocol::PosInfo* temp = pos_info_;
  pos_info_ = nullptr;
  return temp;
}
inline ::Protocol::PosInfo* ObjectInfo::_internal_mutable_pos_info() {
  
  if (pos_info_ == nullptr) {
    auto* p = CreateMaybeMessage<::Protocol::PosInfo>(GetArenaForAllocation());
    pos_info_ = p;
  }
  return pos_info_;
}
inline ::Protocol::PosInfo* ObjectInfo::mutable_pos_info() {
  // @@protoc_insertion_point(field_mutable:Protocol.ObjectInfo.pos_info)
  return _internal_mutable_pos_info();
}
inline void ObjectInfo::set_allocated_pos_info(::Protocol::PosInfo* pos_info) {
  ::PROTOBUF_NAMESPACE_ID::Arena* message_arena = GetArenaForAllocation();
  if (message_arena == nullptr) {
    delete pos_info_;
  }
  if (pos_info) {
    ::PROTOBUF_NAMESPACE_ID::Arena* submessage_arena =
        ::PROTOBUF_NAMESPACE_ID::Arena::InternalHelper<::Protocol::PosInfo>::GetOwningArena(pos_info);
    if (message_arena != submessage_arena) {
      pos_info = ::PROTOBUF_NAMESPACE_ID::internal::GetOwnedMessage(
          message_arena, pos_info, submessage_arena);
    }
    
  } else {
    
  }
  pos_info_ = pos_info;
  // @@protoc_insertion_point(field_set_allocated:Protocol.ObjectInfo.pos_info)
}

#ifdef __GNUC__
  #pragma GCC diagnostic pop
#endif  // __GNUC__
// -------------------------------------------------------------------


// @@protoc_insertion_point(namespace_scope)

}  // namespace Protocol

// @@protoc_insertion_point(global_scope)

#include <google/protobuf/port_undef.inc>
#endif  // GOOGLE_PROTOBUF_INCLUDED_GOOGLE_PROTOBUF_INCLUDED_Struct_2eproto

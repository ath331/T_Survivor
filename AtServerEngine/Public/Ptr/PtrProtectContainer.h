////////////////////////////////////////////////////////////////////////////////////////////////////
// @breif PtrProtectContainer class
////////////////////////////////////////////////////////////////////////////////////////////////////


#pragma once
#include <iostream>
#include <vector>


template < typename T >
class PtrProtectContainer
{
private:
	/// 컨테이너 타입 정의
	using Container = std::vector< T* >;

private:
	/// 컨테이너
	Container m_data;

public:
	/// 생성자
	PtrProtectContainer() {}

	/// 소멸자
	~PtrProtectContainer()
	{
		for ( T* item : m_data )
		{
			if ( item )
				delete item;
		}
	}

	/// Item 추가
	void Register( T* value )
	{
		m_data.push_back( value );
	}

	/// Size 반환
	size_t GetSize() const { return m_data.size(); }

	/// 컨테이너 반환
	const Container& GetContainer() { return m_data; }
};

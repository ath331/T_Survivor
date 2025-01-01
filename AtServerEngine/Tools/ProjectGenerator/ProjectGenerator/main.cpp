/*
	Copyright 2021, Won Seong-Yeon. All Rights Reserved.
		KoreaGameMaker@gmail.com
		github.com/GameForPeople
*/

/*
	#0. VS2022�� ������Ʈ ����, ������Ʈ �������Ͽ� ���Ͽ� ���� ���丮 ������ �°� ��������ִ� ���Դϴ�.

	!0. ������Ʈ�� �ִ� ��θ� ù��° ��� �μ��� ����������մϴ�. ( ������Ʈ�� ���Ͱ� �־�� ���� �� �ֽ��ϴ�. )
	!1. ������Ʈ�� �ִ� ��ο� ������Ʈ ����(.vcxproj)�� �ϳ���� �߰� ��� �μ��� �ʿ�����ϴ�.
	!2. �־��� ��ο� ������Ʈ�� ������ ������ ���������� �������� �ʽ��ϴ�. ( ������Ʈ ������ �������� �˱� ��ƽ��ϴ�. )

	?0. ��� ������Ʈ ��ġ�� �ٲٴ� �۾��� �ʿ��ϴٸ�
	?1. �Է� ������Ʈ ��ġ�� �ٲٴ� �۾��� �ʿ��ϴٸ�
*/

#ifndef WONSY_PCH

#include <iostream>

#include <string>
#include <sstream>

#include <fstream>
#include <filesystem>

#include <vector>
#include <map>

#endif

enum class FILE_TYPE 
{ 
	Header,  // .h, .hpp
	Source,  // .cpp
	Xml,     // .xml
	Cc,      // .cc
	Proto,   // .proto
	Bat,     // .bat
};


using PathCont = std::vector< std::filesystem::path >;


/// ������Ʈ���� pch.cpp�� ���� ���ڿ��� �����ϱ� ���� �����̳�
std::list< std::string > g_pchCpp;


void Log( const std::string& logStr, const bool isTerminate = true )
{
	std::cout << " [WARN] "<< logStr << std::endl;

	if ( isTerminate )
		std::terminate();
}

std::string FindAndReplace( std::string str, const std::string& before, const std::string& after )
{
    size_t startPos = 0;
    while( ( startPos = str.find( before, startPos ) ) != std::string::npos )
    {
        str.replace( startPos, before.length(), after );
        startPos += after.length();
    }
    return str;
}
#define FAR( str ) FindAndReplace( FindAndReplace( str, rootString, "" ), "/", "\\" ) // �� ����;;

int main( int argc, char *argv[] /* 1. ������Ʈ ������ �����ϴ� ��� */ )
{
	// Print UI
	{
		std::cout << ""                                                     << std::endl;
		std::cout << "Project Generator ver 0.1"                            << std::endl; 
		std::cout << "Copyright 2021, Won Seong-Yeon. All Rights Reserved." << std::endl;
		std::cout << "		KoreaGameMaker@gmail.com"                       << std::endl;
		std::cout << "		github.com/GameForPeople"                       << std::endl;
		std::cout << "" << std::endl;
		std::cout << "" << std::endl;
		std::cout << "" << std::endl;
	}

	// ����� �μ��� ����, ������Ʈ ��θ� ���մϴ�.
	const std::string rootString = [ argc, &argv ]() -> std::string
		{
			if ( argc < 2 ) 
			{
				//argv[ 1 ] = (char *)("geneTestProject/geneTestProject/");
				argv[ 1 ] = (char *)("../../../GameServer/");
				//argv[ 2 ] = (char *)("geneTestProject"); 
				//argc = 3;
				Log( "�־��� ����� �μ��� ����, �ϵ� �ڵ��� default ��θ� ����Ͽ����ϴ�.", false );
			}
			return static_cast< std::string >( argv[ 1 ] );
		}();  
	const std::filesystem::path rootPath = rootString; // Root ���

	// �ҽ� ��ũ �˻�
	if ( !std::filesystem::exists( rootPath ) )
	{
		Log( "�ش� ��ΰ� ��ȿ���� �ʾ� �����մϴ�." + rootString );
	}
	
	// ��θ� ���ƴٴϸ�, ������Ʈ �̸��� ã�´�.
	const std::string projectName = argc >= 3 
		? argv[ 2 ]
		: [ &rootPath ]() -> std::string
		{
			for ( const auto& pathIter : std::filesystem::directory_iterator( rootPath ) )
			{
				if ( 
					pathIter.is_regular_file() &&
					pathIter.path().extension().generic_string() == ".vcxproj" )
				{
					return pathIter.path().stem().generic_string();
				}
			}

			Log( "���� ������Ʈ ���� ������ �׾����?" );
			return ""; // for warnning
		}();

	const std::filesystem::path projectPath          = rootPath.generic_string() + projectName + ".vcxproj";         // ���ʷ���Ʈ�� ������Ʈ ���
	const std::filesystem::path filtersPath          = rootPath.generic_string() + projectName + ".vcxproj.filters"; // ���ʷ���Ʈ�� ������Ʈ ���� ���
	const std::string           itemgroupHeadString  = "  <ItemGroup>";
	const std::string           itemgroupTailString  = "  </ItemGroup>";
	// const std::string endString            = "\r\n";

	PathCont                        directoryPathCont; // �ش� ��ο� �����ϴ� ��� ���丮 ��� ����
	std::map< FILE_TYPE, PathCont > filePathCont;      // �ش� ��ο� �����ϴ� ��� ���� ��� -����

	// �ش� ��ο� �����ϴ�, ��� �������� ���ͷ� �����ϰ�, ���ϵ��� ��� ��θ� ����Ʈ�Ѵ�.
	{
		for ( const auto& pathIter : std::filesystem::recursive_directory_iterator( rootPath ) ) 
		{
			if ( pathIter.is_directory() )
			{
				directoryPathCont.emplace_back( pathIter.path() );
			}
			else if ( pathIter.is_regular_file() )
			{
				if (
					pathIter.path().extension().generic_string() == ".h" ||
					pathIter.path().extension().generic_string() == ".hpp" )
				{
					filePathCont[ FILE_TYPE::Header ].emplace_back( pathIter.path() );
				}
				else if ( pathIter.path().extension().generic_string() == ".cpp" )
				{
					filePathCont[ FILE_TYPE::Source ].emplace_back( pathIter.path() );
				}
				else if ( pathIter.path().extension().generic_string() == ".xml" )
				{
					filePathCont[ FILE_TYPE::Xml ].emplace_back( pathIter.path() );
				}
				else if ( pathIter.path().extension().generic_string() == ".cc" )
				{
					filePathCont[ FILE_TYPE::Cc ].emplace_back( pathIter.path() );
				}
				else if ( pathIter.path().extension().generic_string() == ".proto" )
				{
					filePathCont[ FILE_TYPE::Proto ].emplace_back( pathIter.path() );
				}
				else if ( pathIter.path().extension().generic_string() == ".bat" )
				{
					filePathCont[ FILE_TYPE::Bat ].emplace_back( pathIter.path() );
				}
			}
		}
	}

	// vcxproj������ ó���մϴ�.
	{
		std::cout << " [INFO] Write " << projectName << " Vcxproj   Start ";

		// ���� �ִ� ������Ʈ�� ��, �� �κ��� �״�� ����ϱ� ���� ���� ������ �ִ� ������Ʈ ������ �а� ���� ������� �����մϴ�.
		enum class READ_STATE{ Before, NotUse_Check_1, NotUse_Check_2, After };

		READ_STATE                 readState{ READ_STATE::Before };
		std::vector< std::string > preStringCont;
		std::vector< std::string > postStringCont;

		// ������ �ִ� ������Ʈ ������ �н��ϴ�.
		{
			std::ifstream projectFileStream( projectPath );
			if ( !projectFileStream.is_open() )
			{
				Log( "������Ʈ ���� �ȿ����� �׾����?" );
			}

			std::string lineBuffer{};
			while ( projectFileStream )
			{
				// ���پ� �о� ó���Ѵ�.
				std::getline( projectFileStream, lineBuffer );

				// Pch.cpp�� ������ ���� �����ϱ�
				{
					if ( lineBuffer.find( "pch.cpp" ) != std::string::npos )
					{
						while ( true )
						{
							std::getline( projectFileStream, lineBuffer );
							if ( lineBuffer == "    </ClCompile>" )
								break;

							g_pchCpp.push_back( lineBuffer );
						}
					}
				}

				if ( readState == READ_STATE::Before )
				{
					if ( itemgroupHeadString == lineBuffer )
						readState = READ_STATE::NotUse_Check_1;
					else 
						preStringCont.emplace_back( lineBuffer );
				}
				else if ( readState == READ_STATE::NotUse_Check_1 )
				{
					if ( itemgroupTailString == lineBuffer )
					{
						readState = READ_STATE::NotUse_Check_2;
					}
				}
				else if ( readState == READ_STATE::NotUse_Check_2 )
				{
					if ( itemgroupHeadString == lineBuffer )
					{
						readState = READ_STATE::NotUse_Check_1;
					}
					else
					{
						postStringCont.emplace_back( lineBuffer );
						readState = READ_STATE::After;
					}
				}
				else if ( readState == READ_STATE::After )
				{
					// ������ ���� EOF��������, 2����. �ߺ� ���� ���� �ʵ��� ����ó��
					if ( *( --postStringCont.end() ) != lineBuffer )
						postStringCont.emplace_back( lineBuffer );
				}
			}
		}

		// ���� ���ϵ��� �������� ������Ʈ ������ �����Ѵ�.
		{
			std::ofstream fos{ projectPath };

			// Write Pre
			{
				for ( const auto& preString : preStringCont )
				{
					fos
						<< preString << std::endl;
				}
			}

			// Write Item
			{
				for ( const auto& filePathIter : filePathCont )
				{
					fos
						<< itemgroupHeadString << std::endl;

					if ( filePathIter.first == FILE_TYPE::Header )
					{
						for ( const auto& filePath : filePathIter.second )
						{
							fos
								<< "    <ClInclude Include=\"" << FAR( filePath.relative_path().generic_string() ) << "\" />" << std::endl;
						}
					}
					else if ( filePathIter.first == FILE_TYPE::Proto || filePathIter.first == FILE_TYPE::Bat )
					{
						for ( const auto& filePath : filePathIter.second )
						{
							fos
								<< "    <None Include=\"" << FAR( filePath.relative_path().generic_string() ) << "\" />" << std::endl;
						}
					}
					else if ( filePathIter.first == FILE_TYPE::Xml )
					{
						for ( const auto& filePath : filePathIter.second )
						{
							fos
								<< "    <Xml Include=\"" << FAR( filePath.relative_path().generic_string() ) << "\" />" << std::endl;
						}
					}
					else if ( filePathIter.first == FILE_TYPE::Cc )
					{
						for ( const auto& filePath : filePathIter.second )
						{
							std::string name = FAR( filePath.relative_path().generic_string() );

							fos << "    <ClCompile Include=\"" << name << "\">" << std::endl;
							fos << "        <PrecompiledHeader Condition=\"'$(Configuration)|$(Platform)' == 'Debug|x64'\">NotUsing</PrecompiledHeader>" << std::endl;
							fos << "        <PrecompiledHeader Condition=\"'$(Configuration)|$(Platform)' == 'Release|x64'\">NotUsing</PrecompiledHeader>" << std::endl;
							fos << "    </ClCompile>" << std::endl;
						}
					}
					else
					{
						for ( const auto& filePath : filePathIter.second )
						{
							std::string name = FAR( filePath.relative_path().generic_string() );

							// pch.pp�� ������ ���� ��Ʈ������ ����
							if ( name == "pch.cpp" )
							{
								fos
									<< "    <ClCompile Include=\"" << name << "\">" << std::endl;

								for ( const auto& pchCppString : g_pchCpp )
								{
									fos
										<< pchCppString << std::endl;
								}

								fos
									<< "    </ClCompile>" << std::endl;
							}
							else
							{
								fos
									<< "    <ClCompile Include=\"" << name << "\" />" << std::endl;
							}
						}
					}

					fos
						<< itemgroupTailString << std::endl;
				}
			}

			// Write Post
			{
				for ( 
					auto afterStringIter = postStringCont.begin();
					afterStringIter != ( --postStringCont.end() );
					++afterStringIter )
				{
					fos
						<< *afterStringIter << std::endl;
				}

				{
					fos
						<< *( --postStringCont.end() );
				}
			}
		}

		std::cout << " ----------------------------> End! \n";
	}

	{}

	// Filters������ ó���մϴ�.
	{
		std::cout << " [INFO] Write " << projectName << " Filters   Start ";

		// ���� �ִ� ������Ʈ ���� ������ ��, �� �κ��� �״�� ����ϱ� ���� ���� ������ �ִ� ������Ʈ ���� ������ �а� ���� ������� �����մϴ�.
		std::vector< std::string > preStringCont;
		std::vector< std::string > postStringCont;

		// ��û������ �� ���� ��, ���� 2�� �� �Ʒ� 1�� ���´�. -> ���� �ٲ�� ��û���ڵ�˴ϴ�.
		{
			std::ifstream filtersFileStream( filtersPath );
			if ( !filtersFileStream.is_open() )
			{
				Log( "���� ���� �ȿ����� �׾����?" );
			}

			std::vector< std::string > stringCont;

			std::string lineBuffer{};
			while ( filtersFileStream )
			{
				std::getline( filtersFileStream, lineBuffer );
				stringCont.emplace_back( lineBuffer );
			}

			preStringCont.emplace_back( *stringCont.begin()       );
			preStringCont.emplace_back( *( ++stringCont.begin() ) );

			postStringCont.emplace_back( *( --stringCont.end() ) );
		}

		// ������ ������ ���ϴ�.
		{
			std::ofstream fos{ filtersPath };

			// head
			{
				for ( const auto& preString : preStringCont )
				{
					fos
						<< preString << std::endl;
				}
			}

			// Files
			{
				for ( const auto& filePathIter : filePathCont )
				{
					fos
						<< itemgroupHeadString << std::endl;
					
					for ( const auto& filePath : filePathIter.second )
					{
						if ( filePathIter.first == FILE_TYPE::Header )
						{
							// .h
							fos
								<< "    <ClInclude Include=\"" << FAR( filePath.relative_path().generic_string() ) << "\">" << std::endl
								<< "      <Filter>" << FAR( filePath.parent_path().generic_string() ) << "</Filter>" << std::endl
								<< "    </ClInclude>" << std::endl;
						}
						else if ( filePathIter.first == FILE_TYPE::Source || filePathIter.first == FILE_TYPE::Cc )
						{
							// .cpp
							fos
								<< "    <ClCompile Include=\"" << FAR( filePath.relative_path().generic_string() ) << "\">" << std::endl
								<< "      <Filter>" << FAR( filePath.parent_path().generic_string() ) << "</Filter>" << std::endl
								<< "    </ClCompile>" << std::endl;
						}
						else if ( filePathIter.first == FILE_TYPE::Xml )
						{
							// .xml
							fos
								<< "    <Xml Include=\"" << FAR( filePath.relative_path().generic_string() ) << "\">" << std::endl
								<< "      <Filter>" << FAR( filePath.parent_path().generic_string() ) << "</Filter>" << std::endl
								<< "    </Xml>" << std::endl;
						}
						else if ( filePathIter.first == FILE_TYPE::Proto || filePathIter.first == FILE_TYPE::Bat )
						{
							// .proto
							fos
								<< "    <None Include=\"" << FAR( filePath.relative_path().generic_string() ) << "\">" << std::endl
								<< "      <Filter>" << FAR( filePath.parent_path().generic_string() ) << "</Filter>" << std::endl
								<< "    </None>" << std::endl;
						}
					}

					fos
						<< itemgroupTailString << std::endl;
				}
			}
			
			// Folders
			{
				if ( directoryPathCont.size() )
				{
					fos
						<< itemgroupHeadString << std::endl;

					for ( const auto& directoryPath : directoryPathCont )
					{
						fos
						<< "    <Filter Include=\"" << FAR( directoryPath.generic_string() ) << "\">" << std::endl
						<< "    </Filter>"                                                                                         << std::endl;
					}

					fos
						<< itemgroupTailString << std::endl;
				}
			}

			// Fin
			{
				for ( const auto& postString : postStringCont )
				{
					fos
						<< postString; // << '\0';
				}
			}
		}

		std::cout << " ----------------------------> End! \n";
	}
}
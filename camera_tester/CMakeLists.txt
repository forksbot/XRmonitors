cmake_minimum_required(VERSION 3.5)
project(camera_tester LANGUAGES CXX)

################################################################################
# Source

set(SOURCE_FILES
    src/CameraTester.cpp
)


################################################################################
# Build Options

set(CMAKE_CONFIGURATION_TYPES "Debug;Release" CACHE STRING "" FORCE)

if(NOT CMAKE_BUILD_TYPE)
    set(CMAKE_BUILD_TYPE Release)
endif()

# In debug mode, add -DDEBUG
add_compile_options("$<$<CONFIG:DEBUG>:-DDEBUG>")

if(MSVC)
    set(CMAKE_CXX_FLAGS "${CMAKE_CXX_FLAGS} /W4")
else()
    # Warnings
    set(CMAKE_CXX_FLAGS "${CMAKE_CXX_FLAGS} -Wall -Wextra -pedantic")

    # Remove Asio warning
    set(CMAKE_CXX_FLAGS "${CMAKE_CXX_FLAGS} -Wno-unused-local-typedefs")

    # Static library: -fPIC
    set(CMAKE_CXX_FLAGS "${CMAKE_CXX_FLAGS} -fPIC")

    set(CMAKE_CXX_FLAGS_DEBUG "-g -O0 -march=native -fstack-protector")
    set(CMAKE_CXX_FLAGS_RELEASE "-O3 -march=native")
endif()


################################################################################
# Dependencies

include_directories(include)

if(NOT TARGET core)
    add_subdirectory(../core core)
endif()
if(NOT TARGET mrcam_client)
    add_subdirectory(../mrcam_client mrcam_client)
endif()

set(THREADS_PREFER_PTHREAD_FLAG ON)
find_package(Threads REQUIRED)


################################################################################
# Targets

add_executable(camera_tester ${SOURCE_FILES})
target_link_libraries(camera_tester
    core
    mrcam_client
    Threads::Threads)

install(TARGETS camera_tester DESTINATION bin)

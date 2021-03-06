cmake_minimum_required(VERSION 3.5)
project(mrcam_implant LANGUAGES CXX)

################################################################################
# Source

set(INCLUDE_FILES
)

set(SOURCE_FILES
    ${INCLUDE_FILES}
    src/implant_main.cpp
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

if(NOT TARGET protocols)
    add_subdirectory(../protocols protocols)
endif()
if(NOT TARGET detours)
    add_subdirectory(../detours detours)
endif()

set(THREADS_PREFER_PTHREAD_FLAG ON)
find_package(Threads REQUIRED)


################################################################################
# Targets

add_library(mrcam_implant SHARED ${SOURCE_FILES})
target_include_directories(mrcam_implant PUBLIC include)
target_link_libraries(mrcam_implant
    detours
    protocols
    Threads::Threads)

install(TARGETS mrcam_implant DESTINATION lib)
install(FILES ${INCLUDE_FILES} DESTINATION include)

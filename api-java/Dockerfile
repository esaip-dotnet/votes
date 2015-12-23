FROM java:8

# Install maven
RUN apt-get update && apt-get install -y maven

WORKDIR /code

# Download dependencies
ADD pom.xml /code/pom.xml
RUN ["mvn", "dependency:resolve"]
RUN ["mvn", "verify"]

# Add source, compile and package into a jar file.
ADD src /code/src
RUN ["mvn", "package"]

EXPOSE 5004
CMD ["/usr/lib/jvm/java-8-openjdk-amd64/bin/java", "-jar", "target/sparkexample-jar-with-dependencies.jar"]

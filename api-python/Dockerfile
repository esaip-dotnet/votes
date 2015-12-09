FROM ubuntu:trusty
MAINTAINER pro@pichois.fr

RUN apt-get update \
    && apt-get install -y \
       python \
       python-pip \
       wget \
    && pip install Flask

RUN apt-get install -y python-reportlab

COPY src/ .

EXPOSE 5004
ENTRYPOINT ["python", "-u"]
CMD ["app.py"]
